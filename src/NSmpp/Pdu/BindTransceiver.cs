namespace NSmpp.Pdu
{
    internal class BindTransceiver : BindPduBase
    {
        internal override SmppCommand Command
        {
            get { return SmppCommand.BindTransceiver; }
        }
    }
}
