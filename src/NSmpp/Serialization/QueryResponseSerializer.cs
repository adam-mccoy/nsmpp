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

                var pdu = new QueryResponse();
                if (header.Status == SmppStatus.Ok)
                {
                    pdu.MessageId = reader.ReadString();
                    pdu.FinalDate = reader.ReadAbsoluteTime();
                    pdu.MessageState = (MessageState)reader.ReadByte();
                    pdu.ErrorCode = reader.ReadByte();
                }

                return pdu;
            }
            catch (Exception ex)
            {
                throw new PduSerializationException(ex);
            }
        }
    }
}
