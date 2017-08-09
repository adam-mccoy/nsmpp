namespace NSmpp.Pdu
{
    internal struct SmppHeader
    {
        public SmppHeader(int length, SmppCommand command, SmppStatus status, uint sequence)
        {
            Length = length;
            Command = command;
            Status = status;
            Sequence = sequence;
        }

        public int Length { get; private set; }
        public SmppCommand Command { get; private set; }
        public SmppStatus Status { get; private set; }
        public uint Sequence { get; private set; }
    }
}
