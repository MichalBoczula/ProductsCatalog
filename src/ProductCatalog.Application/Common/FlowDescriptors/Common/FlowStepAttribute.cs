namespace ProductCatalog.Application.Common.FlowDescriptors.Common
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class FlowStepAttribute : Attribute
    {
        public FlowStepAttribute(int order, string id, string label)
        {
            Order = order;
            Id = id;
            Label = label;
        }

        public int Order { get; }
        public string Id { get; }
        public string Label { get; }
        public string? RuleId { get; set; }
    }
}
