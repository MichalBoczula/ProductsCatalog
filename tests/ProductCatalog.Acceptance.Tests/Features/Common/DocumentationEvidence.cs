using ProductCatalog.Api;
using ProductCatalog.Application.Common.FlowDescriptors.Abstract;
using ProductCatalog.Application.Common.FlowDescriptors.Common;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;
using System.Reflection;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests.Features.Common;

/// <summary>Links declared source knowledge to actual exchanges; never claims runtime step tracing.</summary>
internal sealed class DocumentationEvidence
{
    private static readonly Lazy<Catalogue> Source = new(Catalogue.Read);
    private readonly CaseLink? _case;
    private readonly string? _technicalBranch;
    private int _primaryCount;

    internal DocumentationEvidence(string scenarioId, string? technicalBranch = null)
    {
        Source.Value.Cases.TryGetValue(scenarioId, out _case);
        _technicalBranch = technicalBranch;
        if (technicalBranch is not null && (!technicalBranch.StartsWith("products.branch.http.", StringComparison.Ordinal) ||
            !Source.Value.Branches.ContainsKey(technicalBranch) || _case is not null))
            throw new InvalidOperationException("Invalid or duplicate technical branch binding.");
    }

    internal void Complete()
    {
        if ((_case is not null || _technicalBranch is not null) && _primaryCount != 1)
            throw new InvalidOperationException($"Expected one primary When exchange, observed {_primaryCount}.");
    }

    internal async Task Observe(ScenarioHttpEvidence evidence, HttpRequestMessage request, HttpResponseMessage response,
        string? operationId, string? exchangeId, string scope, CancellationToken cancellation)
    {
        var catalogue = Source.Value;
        var status = (int)response.StatusCode;
        request.Options.TryGetValue(HttpEvidenceHandler.StepPhaseKey, out var phase);
        var primary = phase == "When" && (_case is not null
            ? operationId == _case.OperationId && status == _case.Status
            : _technicalBranch is not null);
        var branchId = primary ? _case?.BranchId ?? _technicalBranch : null;
        var branch = branchId is null ? null : catalogue.Branches[branchId];
        catalogue.OperationFlows.TryGetValue(operationId ?? "", out var flow);
        var observedRules = new HashSet<string>(StringComparer.Ordinal);
        string? problemCode = null;
        if (response.Content.Headers.ContentType?.MediaType == "application/problem+json")
        {
            using var body = JsonDocument.Parse(await response.Content.ReadAsByteArrayAsync(cancellation));
            problemCode = body.RootElement.GetProperty("code").GetString();
            foreach (var error in body.RootElement.GetProperty("errors").EnumerateArray())
            {
                var name = error.GetProperty("name").GetString()!;
                if (!catalogue.RuleNames.TryGetValue(name, out var ruleId))
                    throw new InvalidOperationException($"Undocumented actual validation rule: {name}");
                observedRules.Add(ruleId);
            }
        }
        var expectedRules = primary ? _case?.RuleIds ?? [] : [];
        var observedRulesDeclared = observedRules.Count == 0 || flow is not null && !observedRules.Except(flow.RuleIds).Any();
        evidence.Record("HTTP documentation links", new
        {
            service = "products",
            scenarioId = evidence.ScenarioId,
            instanceId = evidence.InstanceId,
            exchangeId,
            operationId,
            method = request.Method.Method,
            path = request.RequestUri!.AbsolutePath,
            status,
            stepPhase = phase,
            scope,
            role = primary ? "primary-scenario-outcome" : "supporting-or-technical-exchange",
            linkBasis = "Declared source metadata and scenario expectation joined to an actual HTTP exchange; not runtime branch/step telemetry.",
            nominalFlowId = flow?.Id,
            declaredStepIds = flow?.StepIds ?? [],
            declaredPolicyIds = flow?.PolicyIds ?? [],
            expectedBranchId = branchId,
            expectedBranchStage = branch?.Stage,
            expectedRuleIds = expectedRules,
            observedErrorRuleIds = observedRules.Order(StringComparer.Ordinal).ToArray(),
            observedRulesDeclaredByNominalFlow = observedRulesDeclared,
            problemCode
        });
        if (!observedRulesDeclared)
            throw new InvalidOperationException("Actual rejection contains a rule outside the nominal flow's invoked policies/guards.");
        if (primary)
        {
            Interlocked.Increment(ref _primaryCount);
            if (expectedRules.Except(observedRules).Any())
                throw new InvalidOperationException("Expected rule is absent from the actual rejection response.");
        }
    }

    internal sealed record CaseLink(string ScenarioId, string OperationId, int Status, string BranchId, string[] RuleIds);
    internal sealed record Branch(string Id, string? FlowId, string Stage, string[] RuleIds);
    internal sealed record Flow(string Id, string[] StepIds, string[] PolicyIds, string[] RuleIds);

    internal sealed class Catalogue
    {
        internal Dictionary<string, CaseLink> Cases { get; } = new(StringComparer.Ordinal);
        internal Dictionary<string, Branch> Branches { get; } = new(StringComparer.Ordinal);
        internal Dictionary<string, Flow> OperationFlows { get; } = new(StringComparer.Ordinal);
        internal Dictionary<string, string> RuleNames { get; } = new(StringComparer.Ordinal);

        internal static Catalogue Read()
        {
            var result = new Catalogue();
            var domainTypes = typeof(DocumentationAttribute).Assembly.GetTypes();
            var policies = domainTypes.Where(type => typeof(IValidationPolicyDescriptorProvider).IsAssignableFrom(type) &&
                type.GetCustomAttribute<DocumentationAttribute>() is not null).ToArray();
            var policyRules = new Dictionary<Type, string[]>();
            foreach (var policy in policies)
            {
                var descriptor = ((IValidationPolicyDescriptorProvider)Activator.CreateInstance(policy)!).Describe();
                var ids = new List<string>();
                foreach (var rule in descriptor.Rules)
                {
                    var type = domainTypes.Single(candidate => candidate.Name == rule.RuleName);
                    var id = type.GetCustomAttribute<DocumentationAttribute>()?.Id ?? throw new InvalidOperationException("Missing rule metadata.");
                    if (result.RuleNames.TryGetValue(type.Name, out var existing) && existing != id)
                        throw new InvalidOperationException("Ambiguous rule identity.");
                    result.RuleNames[type.Name] = id;
                    ids.Add(id);
                }
                policyRules.Add(policy, ids.ToArray());
            }
            var flows = new Dictionary<string, Flow>(StringComparer.Ordinal);
            var applicationTypes = typeof(FlowOutcomeAttribute).Assembly.GetTypes();
            foreach (var type in applicationTypes.Concat(typeof(Program).Assembly.GetTypes()))
            {
                var doc = type.GetCustomAttribute<DocumentationAttribute>();
                var isFlow = type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IFlowDescriber<>));
                if (isFlow && doc is not null)
                {
                    var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    var steps = methods.Select(method => method.GetCustomAttribute<FlowStepAttribute>()).OfType<FlowStepAttribute>().OrderBy(step => step.Order).ToArray();
                    foreach (var step in steps.Where(step => step.RuleId is not null))
                    {
                        var action = type.BaseType!.GenericTypeArguments.Single().Name;
                        result.RuleNames.Add(action, step.RuleId!);
                    }
                    var parameterKinds = methods.SelectMany(method => method.GetParameters()).Select(p => p.ParameterType)
                        .Where(p => p.IsGenericType && p.GetGenericTypeDefinition() == typeof(IValidationPolicy<>))
                        .Select(p => p.GenericTypeArguments.Single()).Distinct().ToArray();
                    var invoked = parameterKinds.Select(kind => policies.Single(policy => policy.GetInterfaces().Any(i =>
                        i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidationPolicy<>) && i.GenericTypeArguments.Single() == kind))).ToArray();
                    var ruleIds = invoked.SelectMany(policy => policyRules[policy]).Concat(steps.Where(s => s.RuleId is not null).Select(s => s.RuleId!)).Distinct().ToArray();
                    flows.Add(doc.Id, new Flow(doc.Id, steps.Select(step => doc.Id + "." + step.Id).ToArray(),
                        invoked.Select(policy => policy.GetCustomAttribute<DocumentationAttribute>()!.Id).Order(StringComparer.Ordinal).ToArray(), ruleIds));
                }
                foreach (var outcome in type.GetCustomAttributes<FlowOutcomeAttribute>())
                    result.Branches.Add(outcome.Id, new Branch(outcome.Id, isFlow ? doc!.Id : null, outcome.Stage, outcome.RuleIds));
            }
            using var stream = typeof(DocumentationEvidence).Assembly.GetManifestResourceStream("Products.AcceptanceMatrix")
                ?? throw new InvalidOperationException("Missing embedded acceptance matrix.");
            using var reader = new StreamReader(stream);
            var header = reader.ReadLine()!.Split('\t');
            while (reader.ReadLine() is { } line)
            {
                var cells = line.Split('\t');
                var row = header.Select((name, index) => (name, value: cells[index])).ToDictionary(pair => pair.name, pair => pair.value);
                var entry = new CaseLink(row["scenarioId"], row["operationId"], int.Parse(row["status"], System.Globalization.CultureInfo.InvariantCulture),
                    row["branchId"], row["ruleIds"].Split(',', StringSplitOptions.RemoveEmptyEntries));
                var branch = result.Branches[entry.BranchId];
                if (entry.RuleIds.Except(branch.RuleIds).Any())
                    throw new InvalidOperationException("Scenario rule is not declared by its branch.");
                result.Cases.Add(entry.ScenarioId, entry);
            }
            foreach (var group in result.Cases.Values.GroupBy(c => c.OperationId))
            {
                var flowIds = group.Select(entry => result.Branches[entry.BranchId].FlowId).OfType<string>().Distinct().ToArray();
                if (flowIds.Length > 1)
                    throw new InvalidOperationException("Operation maps to multiple source flows.");
                if (flowIds.Length == 1)
                    result.OperationFlows.Add(group.Key, flows[flowIds[0]]);
            }
            return result;
        }
    }
}
