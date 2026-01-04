namespace ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ValueObjects
{
    public record struct SatelliteNavigationSystem
    {
        public bool GPS { get; private set; }
        public bool AGPS { get; private set; }
        public bool Galileo { get; private set; }
        public bool GLONASS { get; private set; }
        public bool QZSS { get; private set; }

        public SatelliteNavigationSystem(bool gPS, bool aGPS, bool galileo, bool gLONASS, bool qZSS)
        {
            GPS = gPS;
            AGPS = aGPS;
            Galileo = galileo;
            GLONASS = gLONASS;
            QZSS = qZSS;
        }
    }
}
