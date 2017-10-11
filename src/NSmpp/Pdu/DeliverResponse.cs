namespace NSmpp.Pdu
{
    internal class DeliverResponse : ResponsePduBase
    {
        internal override SmppCommand Command
        {
            get { return SmppCommand.DeliverResp; }
        }
    }
}
