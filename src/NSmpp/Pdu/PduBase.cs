namespace NSmpp.Pdu
{
    internal abstract class PduBase
    {
        internal int Length { get; private set; }
        internal SmppCommand Command { get; private set; }
        internal SmppStatus Status { get; private set; }
        internal uint SequenceNumber { get; private set; }

        public PduBase(int length, SmppCommand command,
            SmppStatus status, uint sequenceNumber)
        {
            Length = length;
            Command = command;
            Status = status;
            SequenceNumber = sequenceNumber;
        }
    }
}
