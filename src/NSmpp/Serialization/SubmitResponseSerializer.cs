using System;
using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal class SubmitResponseSerializer : PduSerializer<SubmitResponse>
    {
        internal override byte[] Serialize(SubmitResponse pdu)
        {
            var writer = new PduWriter();
            writer.WritePduHeader(pdu);

            if (pdu.Status == SmppStatus.Ok)
                writer.WriteString(pdu.MessageId);

            var bytes = writer.GetBytes();
            PduWriter.WriteInteger(bytes, 0, bytes.Length);
            return bytes;
        }

        internal override SubmitResponse Deserialize(byte[] bytes)
        {
            try
            {
                var reader = new PduReader(bytes);
                var header = reader.ReadHeader();

                var pdu = new SubmitResponse();
                if (header.Status == SmppStatus.Ok)
                    pdu.MessageId = reader.ReadString();

                return pdu;
            }
            catch (Exception ex)
            {
                throw new PduSerializationException(ex);
            }
        }
    }
}
