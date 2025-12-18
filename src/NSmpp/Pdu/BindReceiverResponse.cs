namespace NSmpp.Pdu;

internal class BindReceiverResponse(
    SmppStatus status,
    string? systemId = null)
    : ResponsePduBase(status)
{
    internal override SmppCommand Command => SmppCommand.BindReceiverResp;

    public string? SystemId { get; } = systemId;

    public override bool Equals(object? obj)
    {
        if (obj == this)
            return true;

        if (obj is not BindReceiverResponse rhs)
            return false;

        return
            rhs.Command == Command &&
            rhs.Status == Status &&
            rhs.SequenceNumber == SequenceNumber &&
            rhs.SystemId == SystemId;
    }

    public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), SystemId);
}