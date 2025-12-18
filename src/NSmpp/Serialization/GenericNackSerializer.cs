using NSmpp.Pdu;

namespace NSmpp.Serialization;

internal class GenericNackSerializer : PduSerializer<GenericNack>
{
    internal override byte[] Serialize(GenericNack pdu)
    {
        var writer = new PduWriter();
        writer.WritePduHeader(pdu);

        return Finalize(writer);
    }

    internal override GenericNack Deserialize(byte[] bytes)
    {
        var reader = new PduReader(bytes);
        reader.ReadInt32(); // read length
        reader.ReadInt32(); // read command
        var status = (SmppStatus)reader.ReadInt32();
        var sequence = (uint)reader.ReadInt32();

        return new GenericNack(status) { SequenceNumber = sequence };
    }
}