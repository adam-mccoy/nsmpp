using NSmpp.Pdu;

namespace NSmpp.Serialization;

internal class QuerySerializer : PduSerializer<Query>
{
    internal override byte[] Serialize(Query pdu)
    {
        var writer = new PduWriter();
        writer.WritePduHeader(pdu);
        writer.WriteString(pdu.MessageId);
        writer.WriteByte((byte)pdu.Source.Ton);
        writer.WriteByte((byte)pdu.Source.Npi);
        writer.WriteString(pdu.Source.Value);

        return Finalize(writer);
    }

    internal override Query Deserialize(byte[] bytes)
    {
        var reader = new PduReader(bytes);
        var length = reader.ReadInt32();
        var command = (SmppCommand)reader.ReadInt32();
        reader.ReadInt32(); // skip status
        var sequence = (uint)reader.ReadInt32();
        var messageId = reader.ReadString() ?? string.Empty;
        var sourceTon = (TypeOfNumber)reader.ReadByte();
        var sourceNpi = (NumericPlanIndicator)reader.ReadByte();
        var sourceAddress = reader.ReadString();

        return new Query(
            sequence,
            messageId,
            new Address(sourceTon, sourceNpi, sourceAddress));
    }
}