namespace ProductCatalog.Application.Common.FlowDescriptors.Common
{
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class FlowStepAttribute : Attribute
    {
        public FlowStepAttribute(int order, string? label = null)
        {
            Order = order;
            Label = label;
        }

        public int Order { get; }
        public string? Label { get; }
    }
}
