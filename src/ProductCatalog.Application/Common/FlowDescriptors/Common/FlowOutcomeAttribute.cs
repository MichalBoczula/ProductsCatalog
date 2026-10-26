namespace ProductCatalog.Application.Common.FlowDescriptors.Common;

/// <summary>Declared branch semantics. Observed behavior is established separately by acceptance assertions.</summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public sealed class FlowOutcomeAttribute(string id, string condition, string effects, params string[] ruleIds) : Attribute
{
    public string Id { get; } = id;
    public string Condition { get; } = condition;
    public string Effects { get; } = effects;
    public string[] RuleIds { get; } = ruleIds;
    public string Stage { get; set; } = "application";
}
