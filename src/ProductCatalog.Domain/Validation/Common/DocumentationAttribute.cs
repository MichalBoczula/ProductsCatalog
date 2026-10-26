namespace ProductCatalog.Domain.Validation.Common;

/// <summary>Stable source identity and intent; this metadata does not execute a validation rule.</summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class DocumentationAttribute(string id, string summary) : Attribute
{
    public string Id { get; } = id;
    public string Summary { get; } = summary;
}
