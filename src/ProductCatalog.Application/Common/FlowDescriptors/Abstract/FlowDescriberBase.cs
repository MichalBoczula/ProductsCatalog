using ProductCatalog.Application.Common.FlowDescriptors.Common;
using System.Collections.Concurrent;
using System.Reflection;

namespace ProductCatalog.Application.Common.FlowDescriptors.Abstract
{
    public abstract class FlowDescriberBase<TAction> : IFlowDescriber<TAction>
{
    private string GetActionName(TAction action) =>
        action?.GetType().Name ?? typeof(TAction).Name;

    public FlowDescription DescribeFlow(TAction action)
    {
        var steps = GetType()
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Select(m => new { Method = m, Attr = m.GetCustomAttribute<FlowStepAttribute>() })
            .Where(x => x.Attr is not null)
            .OrderBy(x => x.Attr!.Order)
            .Select(x => x.Attr!.Label ?? x.Method.Name)
            .ToArray();

        return new FlowDescription
        {
            ActionName = GetActionName(action),
            Steps = steps
        };
    }
}
}
