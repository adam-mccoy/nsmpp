namespace NSmpp.Pdu
{
    internal class CancelResponse : ResponsePduBase
    {
        internal override SmppCommand Command
        {
            get { return SmppCommand.CancelResp; }
        }
    }
}
