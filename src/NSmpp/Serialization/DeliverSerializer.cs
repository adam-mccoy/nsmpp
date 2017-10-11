using NSmpp.Helpers;
using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal class DeliverSerializer : PduSerializer<Deliver>
    {
        internal override byte[] Serialize(Deliver pdu)
        {
            var writer = new PduWriter();
            writer.WritePduHeader(pdu);
            writer.WriteString(pdu.ServiceType);
            writer.WriteAddress(pdu.Source);
            writer.WriteAddress(pdu.Destination);
            writer.WriteByte(PduHelper.BuildEsmClass(pdu.Mode, pdu.Type, pdu.Features));
            writer.WriteByte(pdu.ProtocolId);
            writer.WriteByte((byte)pdu.Priority);
            writer.WriteByte(0x00);
            writer.WriteByte(0x00);
            writer.WriteByte(pdu.RegisteredDelivery);
            writer.WriteByte(0x00);
            writer.WriteByte(pdu.DataCoding);
            writer.WriteByte(0x00);
            writer.WriteByte((byte)pdu.Length);
            writer.WriteString(pdu.ShortMessage);

            return Finalize(writer);
        }

        internal override Deliver Deserialize(byte[] bytes)
        {
            var reader = new PduReader(bytes);
            reader.Skip(12); // skip length, command, and status
            var sequence = (uint)reader.ReadInteger();
            var serviceType = reader.ReadString();
            var source = reader.ReadAddress();
            var dest = reader.ReadAddress();
            var esmClass = reader.ReadByte();
            var protocolId = reader.ReadByte();
            var priority = (PriorityFlag)reader.ReadByte();
            reader.Skip(2);
            var registeredDelivery = reader.ReadByte();
            reader.Skip(1);
            var dataCoding = reader.ReadByte();
            reader.Skip(1);
            var length = (int)reader.ReadByte();
            var shortMessage = reader.ReadString();

            return new Deliver
            {
                SequenceNumber = sequence,
                ServiceType = serviceType,
                Source = source,
                Destination = dest,
                Mode = (MessagingMode)(esmClass & 0x03),
                Type = (MessageType)(esmClass >> 2 & 0x0f),
                Features = (NetworkSpecificFeatures)(esmClass >> 6 & 0x03),
                ProtocolId = protocolId,
                Priority = priority,
                RegisteredDelivery = registeredDelivery,
                DataCoding = dataCoding,
                Length = length,
                ShortMessage = shortMessage
            };
        }
    }
}
