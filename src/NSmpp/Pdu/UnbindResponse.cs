namespace NSmpp.Pdu
{
    internal class UnbindResponse : ResponsePduBase
    {
        public UnbindResponse(SmppStatus status, uint sequenceNumber)
            : base(status, sequenceNumber)
        {
        }

        internal override SmppCommand Command
        {
            get { return SmppCommand.UnbindResp; }
        }

        internal override int Length
        {
            get { return HeaderLength; }
        }
    }
}
