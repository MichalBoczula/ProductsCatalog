using ProductCatalog.Domain.AggregatesModel.Common;
using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ValueObjects;

namespace ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate
{
    public class MobilePhone : AggregateRoot
    {
        public CommonDescription CommonDescription { get; private set; }
        public ElectronicDetails ElectronicDetails { get; private set; }
        public Connectivity Connectivity { get; private set; }
        public SatelliteNavigationSystem SatelliteNavigationSystems { get; private set; }
        public Sensors Sensors { get; private set; }
        public bool FingerPrint { get; private set; }
        public bool FaceId { get; private set; }
        public Guid CategoryId { get; private set; }
        public Money Price { get; private set; }

        private MobilePhone() { }

        public MobilePhone(
            CommonDescription commonDescription,
            ElectronicDetails electronicDetails,
            Connectivity connectivity,
            SatelliteNavigationSystem satelliteNavigationSystems,
            Sensors sensors,
            bool fingerPrint,
            bool faceId,
            Guid categoryId,
            Money price)
        {
            CommonDescription = commonDescription;
            ElectronicDetails = electronicDetails;
            Connectivity = connectivity;
            SatelliteNavigationSystems = satelliteNavigationSystems;
            Sensors = sensors;
            FingerPrint = fingerPrint;
            FaceId = faceId;
            CategoryId = categoryId;
            Price = price;
        }

        public void AssigneNewMobilePhoneInformation(MobilePhone incoming)
        {
            CommonDescription = incoming.CommonDescription;
            ElectronicDetails = incoming.ElectronicDetails;
            Connectivity = incoming.Connectivity;
            SatelliteNavigationSystems = incoming.SatelliteNavigationSystems;
            Sensors = incoming.Sensors;
            FingerPrint = incoming.FingerPrint;
            FaceId = incoming.FaceId;
            CategoryId = incoming.CategoryId;
            Price = incoming.Price;
            SetChangeDate();
        }
    }
}
