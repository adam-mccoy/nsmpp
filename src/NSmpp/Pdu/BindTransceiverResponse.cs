namespace NSmpp.Pdu;

internal class BindTransceiverResponse(
    SmppStatus status,
    string? systemId = null)
    : ResponsePduBase(status)
{
    internal override SmppCommand Command => SmppCommand.BindTransceiverResp;

    public string? SystemId { get; } = systemId;

    public override bool Equals(object? obj)
    {
        if (obj == this)
            return true;

        if (obj is not BindTransceiverResponse rhs)
            return false;

        return
            rhs.Command == Command &&
            rhs.Status == Status &&
            rhs.SequenceNumber == SequenceNumber &&
            rhs.SystemId == SystemId;
    }

    public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), SystemId.GetHashCode());
}