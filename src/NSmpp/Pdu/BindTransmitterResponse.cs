namespace NSmpp.Pdu;

internal class BindTransmitterResponse(
    SmppStatus status,
    string? systemId = null)
    : ResponsePduBase(status)
{
    internal override SmppCommand Command => SmppCommand.BindTransmitterResp;

    public string? SystemId { get; } = systemId;

    public override bool Equals(object? obj)
    {
        if (obj == this)
            return true;

        if (obj is not BindTransmitterResponse rhs)
            return false;

        return
            rhs.Command == Command &&
            rhs.Status == Status &&
            rhs.SequenceNumber == SequenceNumber &&
            rhs.SystemId == SystemId;
    }

    public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), SystemId);
}