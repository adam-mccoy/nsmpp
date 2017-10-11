namespace NSmpp.Pdu
{
    internal class BindReceiver : BindPduBase
    {
        internal override SmppCommand Command
        {
            get { return SmppCommand.BindReceiver; }
        }
    }
}
