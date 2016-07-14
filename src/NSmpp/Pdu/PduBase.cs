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

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;

            var rhs = obj as PduBase;
            if (rhs == null)
                return false;

            return rhs.SequenceNumber == SequenceNumber;
        }

        public override int GetHashCode()
        {
            return SequenceNumber.GetHashCode();
        }
    }
}
