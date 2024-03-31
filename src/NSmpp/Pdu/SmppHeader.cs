namespace NSmpp.Pdu;

internal readonly struct SmppHeader(int length, SmppCommand command, SmppStatus status, uint sequence)
{
    public int Length { get; } = length;
    public SmppCommand Command { get; } = command;
    public SmppStatus Status { get; } = status;
    public uint Sequence { get; } = sequence;
}