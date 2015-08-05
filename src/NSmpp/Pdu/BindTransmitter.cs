namespace NSmpp.Pdu
{
    internal class BindTransmitter : PduBase
    {
        internal string SystemId { get; private set; }
        internal string Password { get; private set; }
        internal string SystemType { get; private set; }
        internal int InterfaceVersion { get; private set; }
        internal int AddressTon { get; private set; }
        internal int AddressNpi { get; private set; }
        internal string AddressRange { get; private set; }

        public BindTransmitter(int length, SmppCommand command,
            SmppStatus status, int sequenceNumber)
            : base(length, command, status, sequenceNumber)
        {
        }
    }
}
