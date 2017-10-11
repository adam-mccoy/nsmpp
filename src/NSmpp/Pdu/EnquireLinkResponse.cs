namespace NSmpp.Pdu
{
    internal class EnquireLinkResponse : ResponsePduBase
    {
        internal override SmppCommand Command
        {
            get { return SmppCommand.EnquireLinkResp; }
        }
    }
}
