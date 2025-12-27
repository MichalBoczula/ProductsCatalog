namespace ProductCatalog.Application.Common.FlowDescriptors.Common
{
    public sealed record FlowDescription
    {
        public required string ActionName { get; init; }

        public IReadOnlyCollection<string> Steps { get; init; } = Array.Empty<string>();
    }
}
