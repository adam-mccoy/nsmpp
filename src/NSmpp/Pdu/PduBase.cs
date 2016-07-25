namespace NSmpp.Pdu
{
    internal abstract class PduBase
    {
        protected const int HeaderLength = 16;

        internal abstract SmppCommand Command { get; }
        internal uint SequenceNumber { get; private set; }

        protected PduBase(uint sequenceNumber)
        {
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
