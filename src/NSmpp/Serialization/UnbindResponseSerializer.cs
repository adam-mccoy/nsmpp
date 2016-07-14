using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal class UnbindResponseSerializer : PduSerializer<UnbindResponse>
    {
        internal override byte[] Serialize(UnbindResponse pdu)
        {
            var writer = new PduWriter(pdu.Length);
            writer.WritePduHeader(pdu);

            return writer.GetBytes();
        }

        internal override UnbindResponse Deserialize(byte[] bytes)
        {
            var reader = new PduReader(bytes);
            int length = reader.ReadInteger();
            var command = (SmppCommand)reader.ReadInteger();
            var status = (SmppStatus)reader.ReadInteger();
            var sequence = (uint)reader.ReadInteger();

            return new UnbindResponse(status, sequence);
        }
    }
}
