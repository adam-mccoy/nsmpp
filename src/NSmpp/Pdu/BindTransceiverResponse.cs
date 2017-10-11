namespace NSmpp.Pdu
{
    internal class BindTransceiverResponse : ResponsePduBase
    {
        internal override SmppCommand Command
        {
            get { return SmppCommand.BindTransceiverResp; }
        }

        public string SystemId { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;

            var rhs = obj as BindTransceiverResponse;
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
