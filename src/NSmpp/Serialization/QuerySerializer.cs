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
            writer.WriteByte((byte)pdu.Source.Ton);
            writer.WriteByte((byte)pdu.Source.Npi);
            writer.WriteString(pdu.Source.Value);

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
                new Address(sourceTon, sourceNpi, sourceAddress));
        }
    }
}
