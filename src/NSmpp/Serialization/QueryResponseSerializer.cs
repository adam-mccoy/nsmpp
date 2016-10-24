using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal class QueryResponseSerializer : PduSerializer<QueryResponse>
    {
        internal override byte[] Serialize(QueryResponse pdu)
        {
            var writer = new PduWriter();
            writer.WritePduHeader(pdu);
            writer.WriteString(pdu.MessageId);
            writer.WriteAbsoluteTime(pdu.FinalDate);
            writer.WriteByte((byte)pdu.MessageState);
            writer.WriteByte((byte)pdu.ErrorCode);

            return Finalize(writer);
        }

        internal override QueryResponse Deserialize(byte[] bytes)
        {
            var reader = new PduReader(bytes);
            var length = reader.ReadInteger();
            var command = (SmppCommand)reader.ReadInteger();
            var status = (SmppStatus)reader.ReadInteger();
            var sequence = (uint)reader.ReadInteger();
            var messageId = reader.ReadString();
            // read date
            var finalDate = reader.ReadAbsoluteTime();
            var messageState = (MessageState)reader.ReadByte();
            var errorCode = reader.ReadByte();

            return new QueryResponse(
                status,
                sequence,
                messageId,
                finalDate,
                messageState,
                errorCode);
        }
    }
}
