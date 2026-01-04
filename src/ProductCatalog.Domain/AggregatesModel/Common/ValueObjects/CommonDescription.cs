namespace ProductCatalog.Domain.AggregatesModel.Common.ValueObjects
{
    public record struct CommonDescription
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string MainPhoto { get; private set; }
        public List<string> OtherPhotos { get; private set; }

        public CommonDescription(string name, string description, string mainPhoto, List<string> otherPhotos)
        {
            Name = name;
            Description = description;
            MainPhoto = mainPhoto;
            OtherPhotos = otherPhotos;
        }
    }
}
