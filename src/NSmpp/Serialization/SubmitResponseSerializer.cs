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
            var reader = new PduReader(bytes);
            var length = reader.ReadInteger();
            var command = (SmppCommand)reader.ReadInteger();
            var status = (SmppStatus)reader.ReadInteger();
            var sequence = (uint)reader.ReadInteger();

            var messageId = status == SmppStatus.Ok ? reader.ReadString() : null;

            return new SubmitResponse(status, sequence, messageId);
        }
    }
}
