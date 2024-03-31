namespace NSmpp.Pdu;

internal abstract class PduBase(uint sequenceNumber)
{
    protected const int HeaderLength = 16;

    internal abstract SmppCommand Command { get; }
    internal uint SequenceNumber { get; } = sequenceNumber;

    public override bool Equals(object? obj)
    {
        if (obj == this)
            return true;

        if (obj is not PduBase rhs)
            return false;

        return rhs.SequenceNumber == SequenceNumber;
    }

    public override int GetHashCode()
    {
        return SequenceNumber.GetHashCode();
    }
}