namespace ProductCatalog.Domain.Validation.Common
{
    public sealed class ResourceNotFoundException : Exception
    {
        public string ActionName { get; private set; }
        public Guid ResourceId { get; private set; }
        public IReadOnlyCollection<Guid> ResourceIds { get; private set; }
        public string ResourceType { get; private set; }

        public ResourceNotFoundException(string actionName, Guid resourceId, string resourceType)
        {
            ActionName = actionName;
            ResourceId = resourceId;
            ResourceIds = [resourceId];
            ResourceType = resourceType;
        }

        public ResourceNotFoundException(string actionName, IReadOnlyCollection<Guid> resourceIds, string resourceType)
        {
            ActionName = actionName;
            ResourceId = resourceIds.FirstOrDefault();
            ResourceIds = resourceIds;
            ResourceType = resourceType;
        }
    }
}
