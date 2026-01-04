namespace ProductCatalog.Domain.AggregatesModel.Common.ValueObjects
{
    public record struct Resolution
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Resolution(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}