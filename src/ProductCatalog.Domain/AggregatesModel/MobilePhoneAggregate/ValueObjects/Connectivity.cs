namespace ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ValueObjects
{
    public record struct Connectivity
    {
        public bool Has5G { get; private set; }
        public bool WiFi { get; private set; }
        public bool NFC { get; private set; }
        public bool Bluetooth { get; private set; }

        public Connectivity(bool has5G, bool wiFi, bool nFC, bool bluetooth)
        {
            Has5G = has5G;
            WiFi = wiFi;
            NFC = nFC;
            Bluetooth = bluetooth;
        }
    }
}
