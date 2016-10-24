using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal class GenericNackSerializer : PduSerializer<GenericNack>
    {
        internal override byte[] Serialize(GenericNack pdu)
        {
            var writer = new PduWriter();
            writer.WritePduHeader(pdu);

            return Finalize(writer);
        }

        internal override GenericNack Deserialize(byte[] bytes)
        {
            var reader = new PduReader(bytes);
            reader.ReadInteger(); // read length
            reader.ReadInteger(); // read command
            var status = (SmppStatus)reader.ReadInteger();
            var sequence = (uint)reader.ReadInteger();

            return new GenericNack(status, sequence);
        }
    }
}
