namespace NSmpp.Pdu;

internal class GenericNack(SmppStatus status, uint sequenceNumber)
    : ResponsePduBase(status, sequenceNumber)
{
    internal override SmppCommand Command => SmppCommand.GenericNack;
}