using System;
using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal class BindReceiverResponseSerializer : PduSerializer<BindReceiverResponse>
    {
        internal override byte[] Serialize(BindReceiverResponse pdu)
        {
            var writer = new PduWriter();
            writer.WritePduHeader(pdu);
            if (pdu.Status == SmppStatus.Ok)
                writer.WriteString(pdu.SystemId);

            var bytes = writer.GetBytes();
            PduWriter.WriteInteger(bytes, 0, bytes.Length);
            return bytes;
        }

        internal override BindReceiverResponse Deserialize(byte[] bytes)
        {
            try
            {
                var reader = new PduReader(bytes);
                var header = reader.ReadHeader();

                var pdu = new BindReceiverResponse
                {
                    Status = header.Status,
                    SequenceNumber = header.Sequence
                };

                if (header.Status == SmppStatus.Ok)
                    pdu.SystemId = reader.ReadString();

                return pdu;
            }
            catch (Exception ex)
            {
                throw new PduSerializationException(ex);
            }
        }
    }
}
