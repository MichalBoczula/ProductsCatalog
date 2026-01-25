namespace ProductCatalog.Domain.AggregatesModel.Common.ValueObjects
{
    public record struct CommonDescription
    {
        public string Name { get; private set; }
        public string Brand { get; private set; }
        public string Description { get; private set; }
        public string MainPhoto { get; private set; }
        public IReadOnlyList<string> OtherPhotos { get; private set; }

        public CommonDescription(string name, string brand, string description, string mainPhoto, IReadOnlyList<string> otherPhotos)
        {
            Name = name;
            Brand = brand;
            Description = description;
            MainPhoto = mainPhoto;
            OtherPhotos = otherPhotos;
        }
    }
}
