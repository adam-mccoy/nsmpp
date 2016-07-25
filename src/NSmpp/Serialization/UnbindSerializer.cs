using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal class UnbindSerializer : PduSerializer<Unbind>
    {
        internal override byte[] Serialize(Unbind pdu)
        {
            var writer = new PduWriter();
            writer.WritePduHeader(pdu);

            var bytes = writer.GetBytes();
            PduWriter.WriteInteger(bytes, 0, bytes.Length);
            return bytes;
        }

        internal override Unbind Deserialize(byte[] bytes)
        {
            var reader = new PduReader(bytes);
            int length = reader.ReadInteger();
            var command = (SmppCommand)reader.ReadInteger();
            reader.ReadInteger(); // skip status
            var sequence = (uint)reader.ReadInteger();

            return new Unbind(sequence);
        }
    }
}
