using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;

namespace ProductCatalog.Performance.BenchmarkTests.ElectronicDetails.Domain
{
    internal static class ElectronicDetailsValidationDataFactory
    {
        private const int Seed = 15;
        private static readonly Random Random = new(Seed);

        public static ElectronicDetails CreateValid()
        {
            return new ElectronicDetails(
                cPU: $"A{Random.Next(15, 18)} Pro",
                gPU: "Apple GPU 6-core",
                ram: "8 GB",
                storage: "128 GB",
                displayType: "OLED Super Retina XDR",
                refreshRateHz: 120,
                screenSizeInches: 6.1m,
                width: 1179,
                height: 2556,
                batteryType: "Li-Ion",
                batteryCapacity: 3274);
        }

        public static ElectronicDetails CreateInvalidSingle()
        {
            var valid = CreateValid();

            return new ElectronicDetails(
                cPU: string.Empty,
                gPU: valid.GPU,
                ram: valid.Ram,
                storage: valid.Storage,
                displayType: valid.DisplayType,
                refreshRateHz: valid.RefreshRateHz,
                screenSizeInches: valid.ScreenSizeInches,
                width: valid.Width,
                height: valid.Height,
                batteryType: valid.BatteryType,
                batteryCapacity: valid.BatteryCapacity);
        }

        public static ElectronicDetails CreateAllInvalid()
        {
            return new ElectronicDetails(
                cPU: string.Empty,
                gPU: string.Empty,
                ram: string.Empty,
                storage: string.Empty,
                displayType: string.Empty,
                refreshRateHz: -1,
                screenSizeInches: -5.0m,
                width: -10,
                height: -10,
                batteryType: string.Empty,
                batteryCapacity: -500);
        }
    }
}
