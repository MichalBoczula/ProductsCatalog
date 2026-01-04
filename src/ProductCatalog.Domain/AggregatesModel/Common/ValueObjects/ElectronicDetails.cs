namespace ProductCatalog.Domain.AggregatesModel.Common.ValueObjects
{
    public record struct ElectronicDetails
    {
        public string CPU { get; private set; }
        public string GPU { get; private set; }
        public string Ram { get; private set; }
        public string Storage { get; private set; }
        public string DisplayType { get; private set; }
        public string RefreshRateHz { get; private set; }
        public string ScreenSizeInches { get; private set; }
        public Resolution Resolution { get; private set; }
        public string BatteryType { get; private set; }
        public string BatteryCapacity { get; private set; }

        public ElectronicDetails(string cPU, string gPU, string ram, string storage, string displayType, string refreshRateHz, string screenSizeInches, Resolution resolution, string batteryType, string batteryCapacity)
        {
            CPU = cPU;
            GPU = gPU;
            Ram = ram;
            Storage = storage;
            DisplayType = displayType;
            RefreshRateHz = refreshRateHz;
            ScreenSizeInches = screenSizeInches;
            Resolution = resolution;
            BatteryType = batteryType;
            BatteryCapacity = batteryCapacity;
        }
    }
}
