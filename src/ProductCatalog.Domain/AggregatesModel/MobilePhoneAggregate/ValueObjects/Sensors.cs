namespace ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ValueObjects
{
    public record struct Sensors
    {
        public bool Accelerometer { get; private set; }
        public bool Gyroscope { get; private set; }
        public bool Proximity { get; private set; }
        public bool Compass { get; private set; }
        public bool Barometer { get; private set; }
        public bool Halla { get; private set; }
        public bool AmbientLight { get; private set; }

        public Sensors(bool accelerometer, bool gyroscope, bool proximity, bool compass, bool barometer, bool halla, bool ambientLight)
        {
            Accelerometer = accelerometer;
            Gyroscope = gyroscope;
            Proximity = proximity;
            Compass = compass;
            Barometer = barometer;
            Halla = halla;
            AmbientLight = ambientLight;
        }
    }
}
