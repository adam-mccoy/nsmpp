using NSmpp.Pdu;

namespace NSmpp.Serialization;

internal class UnbindResponseSerializer : PduSerializer<UnbindResponse>
{
    internal override byte[] Serialize(UnbindResponse pdu)
    {
        var writer = new PduWriter();
        writer.WritePduHeader(pdu);

        var bytes = writer.GetBytes();
        PduWriter.WriteInt32(bytes, 0, bytes.Length);
        return bytes;
    }

    internal override UnbindResponse Deserialize(byte[] bytes)
    {
        var reader = new PduReader(bytes);
        int length = reader.ReadInt32();
        var command = (SmppCommand)reader.ReadInt32();
        var status = (SmppStatus)reader.ReadInt32();
        var sequence = (uint)reader.ReadInt32();

        return new UnbindResponse(status) { SequenceNumber = sequence };
    }
}