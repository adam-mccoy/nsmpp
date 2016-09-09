using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal class QuerySerializer : PduSerializer<Query>
    {
        internal override byte[] Serialize(Query pdu)
        {
            var writer = new PduWriter();
            writer.WritePduHeader(pdu);
            writer.WriteString(pdu.MessageId);
            writer.WriteByte((byte)pdu.SourceTon);
            writer.WriteByte((byte)pdu.SourceNpi);
            writer.WriteString(pdu.SourceAddress);

            return Finalize(writer);
        }

        internal override Query Deserialize(byte[] bytes)
        {
            var reader = new PduReader(bytes);
            var length = reader.ReadInteger();
            var command = (SmppCommand)reader.ReadInteger();
            reader.ReadInteger(); // skip status
            var sequence = (uint)reader.ReadInteger();
            var messageId = reader.ReadString();
            var sourceTon = (TypeOfNumber)reader.ReadByte();
            var sourceNpi = (NumericPlanIndicator)reader.ReadByte();
            var sourceAddress = reader.ReadString();

            return new Query(
                sequence,
                messageId,
                sourceTon,
                sourceNpi,
                sourceAddress);
        }
    }
}
