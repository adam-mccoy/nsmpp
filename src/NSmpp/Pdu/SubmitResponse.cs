namespace NSmpp.Pdu
{
    internal class SubmitResponse : ResponsePduBase
    {
        internal override SmppCommand Command
        {
            get { return SmppCommand.SubmitResp; }
        }

        internal string MessageId { get; set; }
    }
}
