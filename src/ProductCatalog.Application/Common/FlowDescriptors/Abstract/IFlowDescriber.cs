using ProductCatalog.Application.Common.FlowDescriptors.Common;

namespace ProductCatalog.Application.Common.FlowDescriptors.Abstract
{
    public interface IFlowDescriber<TAction>
    {
        FlowDescription DescribeFlow(TAction action);
    }
}
