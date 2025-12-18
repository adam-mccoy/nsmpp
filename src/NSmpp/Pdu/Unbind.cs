namespace NSmpp.Pdu;

internal class Unbind : PduBase
{
    internal override SmppCommand Command => SmppCommand.Unbind;
}