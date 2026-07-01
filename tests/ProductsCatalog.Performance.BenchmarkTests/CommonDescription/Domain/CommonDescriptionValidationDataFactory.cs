using CommonDescriptionValueObject = ProductCatalog.Domain.AggregatesModel.Common.ValueObjects.CommonDescription;

namespace ProductCatalog.Performance.BenchmarkTests.CommonDescription.Domain
{
    internal static class CommonDescriptionValidationDataFactory
    {
        public const int Seed = 20260625;

        public static readonly IReadOnlyList<string> ValidOtherPhotos =
        [
            $"https://store.com/photos/common-description-{Seed}-side.jpg"
        ];

        public static CommonDescriptionValueObject CreateValid()
        {
            return new CommonDescriptionValueObject(
                name: $"Benchmark Product {Seed}",
                brand: "Benchmark Brand",
                description: "Benchmark product description used for common description validation policy performance tests.",
                mainPhoto: $"https://store.com/photos/common-description-{Seed}-main.jpg",
                otherPhotos: ValidOtherPhotos);
        }

        public static CommonDescriptionValueObject CreateInvalidSingle()
        {
            var valid = CreateValid();

            return new CommonDescriptionValueObject(
                name: string.Empty,
                brand: valid.Brand,
                description: valid.Description,
                mainPhoto: valid.MainPhoto,
                otherPhotos: valid.OtherPhotos);
        }

        public static CommonDescriptionValueObject CreateAllInvalid()
        {
            return new CommonDescriptionValueObject(
                name: string.Empty,
                brand: string.Empty,
                description: string.Empty,
                mainPhoto: string.Empty,
                otherPhotos: [string.Empty]);
        }
    }
}
