namespace NSmpp.Pdu
{
    internal class BindTransceiver : PduBase
    {
        internal override int Length
        {
            get
            {
                return HeaderLength +
                       SystemId.Length + 1 +
                       Password.Length + 1 +
                       (SystemType?.Length ?? 0) + 1 +
                       3 +
                       (AddressRange?.Length ?? 0) + 1;
            }
        }

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
            SmppStatus status,
            uint sequenceNumber,
            string systemId,
            string password,
            string systemType,
            byte interfaceVersion,
            TypeOfNumber addressTon,
            NumericPlanIndicator addressNpi,
            string addressRange)
            : base(status, sequenceNumber)
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
