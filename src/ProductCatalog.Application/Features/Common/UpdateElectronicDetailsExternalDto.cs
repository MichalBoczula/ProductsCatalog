namespace ProductCatalog.Application.Features.Common
{
    public sealed record UpdateElectronicDetailsExternalDto(
        string CPU,
        string GPU,
        string Ram,
        string Storage,
        string DisplayType,
        int RefreshRateHz,
        decimal ScreenSizeInches,
        int Width,
        int Height,
        string BatteryType,
        int BatteryCapacity);
}
