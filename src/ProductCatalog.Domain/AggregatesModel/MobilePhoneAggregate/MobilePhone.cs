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
        public string Camera { get; private set; } = null!;
        public bool FingerPrint { get; private set; }
        public bool FaceId { get; private set; }
        public Money Price { get; private set; }
        public string Description2 { get; private set; } = null!;
        public string Description3 { get; private set; } = null!;

        private MobilePhone() { }

        public MobilePhone(
            CommonDescription commonDescription,
            ElectronicDetails electronicDetails,
            Connectivity connectivity,
            SatelliteNavigationSystem satelliteNavigationSystems,
            Sensors sensors,
            string camera,
            bool fingerPrint,
            bool faceId,
            Money price,
            string description2,
            string description3)
        {
            CommonDescription = commonDescription;
            ElectronicDetails = electronicDetails;
            Connectivity = connectivity;
            SatelliteNavigationSystems = satelliteNavigationSystems;
            Sensors = sensors;
            Camera = camera;
            FingerPrint = fingerPrint;
            FaceId = faceId;
            Price = price;
            Description2 = description2;
            Description3 = description3;
        }

        public void AssigneNewMobilePhoneInformation(MobilePhone incoming)
        {
            CommonDescription = incoming.CommonDescription;
            ElectronicDetails = incoming.ElectronicDetails;
            Connectivity = incoming.Connectivity;
            SatelliteNavigationSystems = incoming.SatelliteNavigationSystems;
            Sensors = incoming.Sensors;
            Camera = incoming.Camera;
            FingerPrint = incoming.FingerPrint;
            FaceId = incoming.FaceId;
            Price = incoming.Price;
            Description2 = incoming.Description2;
            Description3 = incoming.Description3;
            SetChangeDate();
        }

        public bool HasSameInformation(MobilePhone other)
        {
            return CommonDescription.Name == other.CommonDescription.Name
                && CommonDescription.Brand == other.CommonDescription.Brand
                && CommonDescription.Description == other.CommonDescription.Description
                && CommonDescription.MainPhoto == other.CommonDescription.MainPhoto
                && CommonDescription.OtherPhotos.SequenceEqual(other.CommonDescription.OtherPhotos)
                && ElectronicDetails == other.ElectronicDetails
                && Connectivity == other.Connectivity
                && SatelliteNavigationSystems == other.SatelliteNavigationSystems
                && Sensors == other.Sensors
                && Camera == other.Camera
                && FingerPrint == other.FingerPrint
                && FaceId == other.FaceId
                && Price == other.Price
                && Description2 == other.Description2
                && Description3 == other.Description3;
        }
    }
}
