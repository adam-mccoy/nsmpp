using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal class UnbindResponseSerializer : PduSerializer<UnbindResponse>
    {
        internal override byte[] Serialize(UnbindResponse pdu)
        {
            var writer = new PduWriter();
            writer.WritePduHeader(pdu);

            var bytes = writer.GetBytes();
            PduWriter.WriteInteger(bytes, 0, bytes.Length);
            return bytes;
        }

        internal override UnbindResponse Deserialize(byte[] bytes)
        {
            var reader = new PduReader(bytes);
            int length = reader.ReadInteger();
            var command = (SmppCommand)reader.ReadInteger();
            var status = (SmppStatus)reader.ReadInteger();
            var sequence = (uint)reader.ReadInteger();

            return new UnbindResponse
            {
                SequenceNumber = sequence,
                Status = status
            };
        }
    }
}
