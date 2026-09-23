namespace ProductCatalog.Domain.AggregatesModel.Common
{
    public abstract class AggregateRoot
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public bool IsActive { get; private set; } = true;
        public DateTime ChangedAt { get; private set; } = DateTime.UtcNow;


        public void SetChangeDate()
        {
            var now = DateTime.UtcNow;
            ChangedAt = now > ChangedAt ? now : ChangedAt.AddTicks(1);
        }

        public void Deactivate()
        {
            IsActive = false;
            SetChangeDate();
        }
    }
}
