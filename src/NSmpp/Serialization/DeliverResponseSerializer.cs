using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal class DeliverResponseSerializer : PduSerializer<DeliverResponse>
    {
        internal override byte[] Serialize(DeliverResponse pdu)
        {
            var writer = new PduWriter();
            writer.WritePduHeader(pdu);
            writer.WriteByte(0x00);

            return Finalize(writer);
        }

        internal override DeliverResponse Deserialize(byte[] bytes)
        {
            var reader = new PduReader(bytes);
            reader.Skip(8); // skip length and command
            var status = (SmppStatus)reader.ReadInteger();
            var sequence = (uint)reader.ReadInteger();

            return new DeliverResponse(status, sequence);
        }
    }
}
