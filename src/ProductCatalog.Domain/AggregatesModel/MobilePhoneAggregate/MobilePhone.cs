using ProductCatalog.Domain.AggregatesModel.Common;
using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ValueObjects;

namespace ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate
{
    public class MobilePhone : AggregateRoot
    {
        public CommonDescription CommonDescription { get; private set; }
        public Guid ProductId { get; private set; }
        public ElectronicDetails ElectronicDetails { get; private set; }
        public Connectivity Connectivity { get; private set; }
        public SatelliteNavigationSystem SatelliteNavigationSystem { get; private set; }
        public Sensors Sensors { get; private set; }
        public bool FingerPrintId { get; private set; }
        public bool FaceId { get; private set; }
        public Money Price { get; private set; }
    }
}
