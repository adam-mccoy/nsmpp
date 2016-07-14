namespace NSmpp.Pdu
{
    internal abstract class PduBase
    {
        protected const int HeaderLength = 16;

        internal abstract int Length { get; }
        internal abstract SmppCommand Command { get; }
        internal SmppStatus Status { get; private set; }
        internal uint SequenceNumber { get; private set; }

        protected PduBase(SmppStatus status, uint sequenceNumber)
        {
            Status = status;
            SequenceNumber = sequenceNumber;
        }
    }
}
