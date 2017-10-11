namespace NSmpp.Pdu
{
    internal abstract class BindPduBase : PduBase
    {
        internal string SystemId { get; set; }
        internal string Password { get; set; }
        internal string SystemType { get; set; }
        internal byte InterfaceVersion { get; set; }
        internal TypeOfNumber AddressTon { get; set; }
        internal NumericPlanIndicator AddressNpi { get; set; }
        internal string AddressRange { get; set; }
    }
}
