using ProductCatalog.Application.Common.FlowDescriptors.Abstract;
using ProductCatalog.Application.Common.FlowDescriptors.Common;
using ProductCatalog.Application.Features.MobilePhones.Commands.DeleteMobilePhone;
using ProductCatalog.Application.Features.MobilePhones.Commands.UpdateMobilePhone;
using ProductCatalog.Domain.Validation.Common;
using Shouldly;
using System.Reflection;

namespace ProductsCatalog.Application.UnitTests.Features.MobilePhones.Commands;

public class MobilePhoneWriteFlowDescriptionTests
{
    [Fact]
    public void UpdateDescription_ShouldShowUnchangedBranchBeforeWrites()
    {
        var descriptor = new UpdateMobilePhoneCommandFlowDescribtor();
        var steps = Steps(descriptor.GetType());
        var branch = steps.Single(step => step.Id == "same-information");
        branch.Label.ShouldContain("information is unchanged");
        branch.Order.ShouldBeLessThan(steps.Single(step => step.Id == "stage-history").Order);
        branch.Order.ShouldBeLessThan(steps.Single(step => step.Id == "save").Order);
        descriptor.DescribeFlow(new UpdateMobilePhoneCommand(Guid.Empty, null!)).Steps.ShouldBe(steps.Select(step => step.Label));
    }

    [Fact]
    public void DeleteDescription_ShouldShowAlreadyInactiveBranchBeforeWrites()
    {
        var descriptor = new DeleteMobilePhoneCommandFlowDescribtor();
        var steps = Steps(descriptor.GetType());
        var branch = steps.Single(step => step.Id == "already-inactive");
        branch.Label.ShouldContain("already inactive");
        branch.Order.ShouldBeLessThan(steps.Single(step => step.Id == "stage-history").Order);
        branch.Order.ShouldBeLessThan(steps.Single(step => step.Id == "save").Order);
        descriptor.DescribeFlow(new DeleteMobilePhoneCommand(Guid.Empty)).Steps.ShouldBe(steps.Select(step => step.Label));
    }

    [Fact]
    public void EveryPublishedFlowHasStableIdsAndMeaningfulStepAndBranchDescriptions()
    {
        var ids = new HashSet<string>();
        var flows = typeof(FlowOutcomeAttribute).Assembly.GetTypes().Where(type =>
            type.GetCustomAttribute<DocumentationAttribute>() is not null && type.GetInterfaces().Any(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IFlowDescriber<>))).ToArray();
        flows.Length.ShouldBe(9);
        var stepCount = 0;
        foreach (var type in flows)
        {
            var metadata = type.GetCustomAttribute<DocumentationAttribute>()!;
            ids.Add(metadata.Id).ShouldBeTrue();
            metadata.Summary.ShouldContain(" ");
            var steps = Steps(type);
            steps.Select(s => s.Order).ShouldBe(Enumerable.Range(1, steps.Length));
            foreach (var step in steps)
            {
                ids.Add(metadata.Id + "." + step.Id).ShouldBeTrue();
                step.Label.ShouldContain(" ");
                stepCount++;
            }
            var branches = type.GetCustomAttributes<FlowOutcomeAttribute>().ToArray();
            branches.ShouldNotBeEmpty();
            foreach (var branch in branches)
            {
                ids.Add(branch.Id).ShouldBeTrue();
                branch.Condition.ShouldNotBeNullOrWhiteSpace();
                branch.Effects.ShouldNotBeNullOrWhiteSpace();
            }
        }
        stepCount.ShouldBe(57);
    }

    private static FlowStepAttribute[] Steps(Type type) => type.GetMethods()
        .Select(method => method.GetCustomAttribute<FlowStepAttribute>()).OfType<FlowStepAttribute>()
        .OrderBy(step => step.Order).ToArray();
}
