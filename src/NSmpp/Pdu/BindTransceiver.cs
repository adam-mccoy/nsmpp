namespace NSmpp.Pdu
{
    internal class BindTransceiver : PduBase
    {
        internal override SmppCommand Command
        {
            get { return SmppCommand.BindTransceiver; }
        }

        internal string SystemId { get; private set; }
        internal string Password { get; private set; }
        internal string SystemType { get; private set; }
        internal byte InterfaceVersion { get; private set; }
        internal TypeOfNumber AddressTon { get; private set; }
        internal NumericPlanIndicator AddressNpi { get; private set; }
        internal string AddressRange { get; private set; }

        public BindTransceiver(
            uint sequenceNumber,
            string systemId,
            string password,
            string systemType,
            byte interfaceVersion,
            TypeOfNumber addressTon,
            NumericPlanIndicator addressNpi,
            string addressRange)
            : base(sequenceNumber)
        {
            SystemId = systemId;
            Password = password;
            SystemType = systemType;
            InterfaceVersion = interfaceVersion;
            AddressTon = addressTon;
            AddressNpi = addressNpi;
            AddressRange = AddressRange;
        }
    }
}
