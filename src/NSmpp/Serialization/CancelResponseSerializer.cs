using NSmpp.Pdu;

namespace NSmpp.Serialization;

internal class CancelResponseSerializer : PduSerializer<CancelResponse>
{
    internal override byte[] Serialize(CancelResponse pdu)
    {
        var writer = new PduWriter();
        writer.WritePduHeader(pdu);

        return Finalize(writer);
    }

    internal override CancelResponse Deserialize(byte[] bytes)
    {
        var reader = new PduReader(bytes);
        reader.Skip(8); // skip length and command
        var status = (SmppStatus)reader.ReadInt32();
        var sequence = (uint)reader.ReadInt32();

        return new CancelResponse(status, sequence);
    }
}