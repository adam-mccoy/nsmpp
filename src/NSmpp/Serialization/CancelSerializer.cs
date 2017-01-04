using System;
using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal class CancelSerializer : PduSerializer<Cancel>
    {
        internal override byte[] Serialize(Cancel pdu)
        {
            var writer = new PduWriter();
            writer.WritePduHeader(pdu);
            writer.WriteString(pdu.ServiceType);
            writer.WriteString(pdu.MessageId);
            writer.WriteAddress(pdu.Source);
            writer.WriteAddress(pdu.Destination);
            return Finalize(writer);
        }

        internal override Cancel Deserialize(byte[] bytes)
        {
            var reader = new PduReader(bytes);
            reader.Skip(12);
            var sequence = (uint)reader.ReadInteger();
            var serviceType = reader.ReadString();
            var messageId = reader.ReadString();
            var source = reader.ReadAddress();
            var dest = reader.ReadAddress();

            return new Cancel(sequence, serviceType, messageId, source, dest);
        }
    }
}
