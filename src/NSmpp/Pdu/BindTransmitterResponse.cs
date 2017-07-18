namespace NSmpp.Pdu
{
    internal class BindTransmitterResponse : ResponsePduBase
    {
        public BindTransmitterResponse(SmppStatus status, uint sequenceNumber)
            : base(status, sequenceNumber)
        {
        }

        public BindTransmitterResponse(
            SmppStatus status,
            uint sequenceNumber,
            string systemId)
            : base(status, sequenceNumber)
        {
            SystemId = systemId;
        }

        internal override SmppCommand Command
        {
            get { return SmppCommand.BindTransmitterResp; }
        }

        public string SystemId { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;

            var rhs = obj as BindTransmitterResponse;
            if (rhs == null)
                return false;

            return
                rhs.Command == Command &&
                rhs.Status == Status &&
                rhs.SequenceNumber == SequenceNumber &&
                rhs.SystemId == SystemId;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() * 17 ^ SystemId.GetHashCode();
        }
    }
}
