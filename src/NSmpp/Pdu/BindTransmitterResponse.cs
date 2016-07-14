namespace NSmpp.Pdu
{
    internal class BindTransmitterResponse : PduBase
    {
        internal override int Length
        {
            get { return HeaderLength + (SystemId?.Length ?? 0) + 1; }
        }

        internal override SmppCommand Command
        {
            get { return SmppCommand.BindTransmitterResp; }
        }

        public string SystemId { get; private set; }

        public BindTransmitterResponse(
            SmppStatus status,
            uint sequenceNumber,
            string systemId)
            : base(status, sequenceNumber)
        {
            SystemId = systemId;
        }
    }
}
