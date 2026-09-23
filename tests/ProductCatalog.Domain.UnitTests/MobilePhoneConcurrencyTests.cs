using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ValueObjects;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests;

public sealed class MobilePhoneConcurrencyTests
{
    [Fact]
    public void UnchangedInformation_ShouldComparePhotoContentsAndKeepTimestampForNoOp()
    {
        var current = Phone(["photo.jpg"]);
        var same = Phone(["photo.jpg"]);
        var different = Phone(["other.jpg"]);
        var before = current.ChangedAt;

        current.HasSameInformation(same).ShouldBeTrue();
        current.HasSameInformation(different).ShouldBeFalse();
        current.ChangedAt.ShouldBe(before);
    }

    [Fact]
    public void ChangedWrite_ShouldAdvanceConcurrencyTimestamp()
    {
        var current = Phone(["photo.jpg"]);
        var before = current.ChangedAt;

        current.AssigneNewMobilePhoneInformation(Phone(["other.jpg"]));

        current.ChangedAt.ShouldBeGreaterThan(before);
    }

    private static MobilePhone Phone(IReadOnlyList<string> photos) => new(
        new CommonDescription("Phone", "Brand", "Description", "main.jpg", photos),
        new ElectronicDetails("CPU", "GPU", "8 GB", "128 GB", "OLED", 120, 6.1m, 71, 146, "Li-Ion", 4000),
        new Connectivity(true, true, true, true),
        new SatelliteNavigationSystem(true, true, true, false, false),
        new Sensors(true, true, true, true, false, false, true),
        "48 MP", true, false, new Money(999.99m, "USD"), "Second", "Third");
}
