namespace ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ReadModel
{
    public sealed record MobilePhoneReadModel
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required string MainPhoto { get; init; }
        public required string OtherPhotos { get; init; }
        public required bool FingerPrint { get; init; }
        public required bool FaceId { get; init; }
        public required Guid CategoryId { get; init; }
        public required decimal PriceAmount { get; init; }
        public required string PriceCurrency { get; init; }
        public required bool IsActive { get; init; }
    }
}
