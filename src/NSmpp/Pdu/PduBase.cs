namespace NSmpp.Pdu;

internal abstract class PduBase
{
    internal abstract SmppCommand Command { get; }
    internal uint SequenceNumber { get; set; }
}