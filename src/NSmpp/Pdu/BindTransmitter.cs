namespace NSmpp.Pdu
{
    internal class BindTransmitter : PduBase
    {
        internal string SystemId { get; private set; }
        internal string Password { get; private set; }
        internal string SystemType { get; private set; }
        internal byte InterfaceVersion { get; private set; }
        internal byte AddressTon { get; private set; }
        internal byte AddressNpi { get; private set; }
        internal string AddressRange { get; private set; }

        public BindTransmitter(
            int length,
            SmppCommand command,
            SmppStatus status,
            int sequenceNumber,
            string systemId,
            string password,
            string systemType,
            byte interfaceVersion,
            byte addressTon,
            byte addressNpi,
            string addressRange)
            : base(length, command, status, sequenceNumber)
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
