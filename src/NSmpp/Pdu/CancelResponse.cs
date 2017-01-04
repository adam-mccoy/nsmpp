namespace NSmpp.Pdu
{
    internal class CancelResponse : ResponsePduBase
    {
        public CancelResponse(SmppStatus status, uint sequenceNumber)
            : base(status, sequenceNumber)
        {
        }

        internal override SmppCommand Command
        {
            get { return SmppCommand.CancelResp; }
        }
    }
}
