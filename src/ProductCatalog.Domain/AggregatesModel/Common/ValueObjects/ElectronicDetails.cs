namespace ProductCatalog.Domain.AggregatesModel.Common.ValueObjects
{
    public record struct ElectronicDetails
    {
        public string CPU { get; private set; }
        public string GPU { get; private set; }
        public string Ram { get; private set; }
        public string Storage { get; private set; }
        public string DisplayType { get; private set; }
        public int RefreshRateHz { get; private set; }
        public decimal ScreenSizeInches { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public string BatteryType { get; private set; }
        public int BatteryCapacity { get; private set; }


        public ElectronicDetails(string cPU, string gPU, string ram, string storage, string displayType, int refreshRateHz, decimal screenSizeInches, int width, int height, string batteryType, int batteryCapacity)
        {
            CPU = cPU;
            GPU = gPU;
            Ram = ram;
            Storage = storage;
            DisplayType = displayType;
            RefreshRateHz = refreshRateHz;
            ScreenSizeInches = screenSizeInches;
            Width = width;
            Height = height;
            BatteryType = batteryType;
            BatteryCapacity = batteryCapacity;
        }

    }
}
