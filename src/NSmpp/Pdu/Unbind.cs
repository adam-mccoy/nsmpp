namespace NSmpp.Pdu;

internal class Unbind(uint sequenceNumber) : PduBase(sequenceNumber)
{
    internal override SmppCommand Command => SmppCommand.Unbind;
}