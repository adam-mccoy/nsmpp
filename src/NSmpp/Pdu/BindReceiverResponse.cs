namespace NSmpp.Pdu
{
    internal class BindReceiverResponse : PduBase
    {
        internal override int Length
        {
            get { return HeaderLength + (SystemId?.Length ?? 0) + 1; }
        }

        internal override SmppCommand Command
        {
            get { return SmppCommand.BindReceiverResp; }
        }

        public string SystemId { get; private set; }

        public BindReceiverResponse(
            SmppStatus status,
            uint sequenceNumber,
            string systemId)
            : base(status, sequenceNumber)
        {
            SystemId = systemId;
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;

            var rhs = obj as BindReceiverResponse;
            if (rhs == null)
                return false;

            return
                rhs.Length == Length &&
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
