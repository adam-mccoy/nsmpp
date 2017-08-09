namespace NSmpp.Pdu
{
    internal class SubmitResponse : ResponsePduBase
    {
        public SubmitResponse(SmppStatus status, uint sequenceNumber)
            : base(status, sequenceNumber)
        {
        }

        public SubmitResponse(
            SmppStatus status,
            uint sequenceNumber,
            string messageId)
            : base(status, sequenceNumber)
        {
            MessageId = messageId;
        }

        internal override SmppCommand Command
        {
            get { return SmppCommand.SubmitResp; }
        }

        internal string MessageId { get; private set; }
    }
}
