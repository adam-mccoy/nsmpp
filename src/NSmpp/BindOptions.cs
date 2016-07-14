namespace NSmpp
{
    public class BindOptions
    {
        public string SystemType { get; set; } = null;
        public byte InterfaceVersion { get; set; } = 0x34;
        public TypeOfNumber Ton { get; set; } = TypeOfNumber.Unknown;
        public NumericPlanIndicator Npi { get; set; } = NumericPlanIndicator.Unknown;
        public string AddressRange { get; set; } = null;
    }
}
