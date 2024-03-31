using NSmpp.Pdu;

namespace NSmpp.Serialization;

internal class UnbindSerializer : PduSerializer<Unbind>
{
    internal override byte[] Serialize(Unbind pdu)
    {
        var writer = new PduWriter();
        writer.WritePduHeader(pdu);

        var bytes = writer.GetBytes();
        PduWriter.WriteInt32(bytes, 0, bytes.Length);
        return bytes;
    }

    internal override Unbind Deserialize(byte[] bytes)
    {
        var reader = new PduReader(bytes);
        var length = reader.ReadInt32();
        var command = (SmppCommand)reader.ReadInt32();
        reader.ReadInt32(); // skip status
        var sequence = (uint)reader.ReadInt32();

        return new Unbind(sequence);
    }
}