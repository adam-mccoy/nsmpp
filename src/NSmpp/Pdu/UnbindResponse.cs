namespace NSmpp.Pdu
{
    internal class UnbindResponse : ResponsePduBase
    {
        internal override SmppCommand Command
        {
            get { return SmppCommand.UnbindResp; }
        }
    }
}
