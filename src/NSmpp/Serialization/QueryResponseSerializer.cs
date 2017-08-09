using System;
using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal class QueryResponseSerializer : PduSerializer<QueryResponse>
    {
        internal override byte[] Serialize(QueryResponse pdu)
        {
            var writer = new PduWriter();
            writer.WritePduHeader(pdu);

            if (pdu.Status == SmppStatus.Ok)
            {
                writer.WriteString(pdu.MessageId);
                writer.WriteAbsoluteTime(pdu.FinalDate);
                writer.WriteByte((byte)pdu.MessageState);
                writer.WriteByte((byte)pdu.ErrorCode);
            }

            return Finalize(writer);
        }

        internal override QueryResponse Deserialize(byte[] bytes)
        {
            try
            {
                var reader = new PduReader(bytes);
                var header = reader.ReadHeader();

                if (header.Status != SmppStatus.Ok)
                    return new QueryResponse(header.Status, header.Sequence);

                var messageId = reader.ReadString();
                var finalDate = reader.ReadAbsoluteTime();
                var messageState = (MessageState)reader.ReadByte();
                var errorCode = reader.ReadByte();

                return new QueryResponse(
                    header.Status,
                    header.Sequence,
                    messageId,
                    finalDate,
                    messageState,
                    errorCode);
            }
            catch (Exception ex)
            {
                throw new PduSerializationException(ex);
            }
        }
    }
}
