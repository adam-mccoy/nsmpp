using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal class SubmitSerializer : PduSerializer<Submit>
    {
        internal override byte[] Serialize(Submit pdu)
        {
            var writer = new PduWriter();
            writer.WritePduHeader(pdu);
            writer.WriteString(pdu.ServiceType);
            writer.WriteByte((byte)pdu.Source.Ton);
            writer.WriteByte((byte)pdu.Source.Npi);
            writer.WriteString(pdu.Source.Value);
            writer.WriteByte((byte)pdu.Destination.Ton);
            writer.WriteByte((byte)pdu.Destination.Npi);
            writer.WriteString(pdu.Destination.Value);
            writer.WriteByte((byte)pdu.EsmClass);
            writer.WriteByte((byte)pdu.ProtocolId);
            writer.WriteByte((byte)pdu.PriorityFlag);
            writer.WriteString(pdu.ScheduleDeliveryTime);
            writer.WriteString(pdu.ValidityPeriod);
            writer.WriteByte(pdu.RegisteredDelivery); // registered delivery
            writer.WriteByte(pdu.ReplaceIfPresent ? 0x01 : 0x00); // replace if present
            writer.WriteByte(0x00); // data coding
            writer.WriteByte(pdu.DefaultMessageId); // sm default msg id
            writer.WriteByte((byte)pdu.ShortMessage.Length);
            writer.WriteBytes(pdu.ShortMessage);

            return Finalize(writer);
        }

        internal override Submit Deserialize(byte[] bytes)
        {
            var reader = new PduReader(bytes);
            int length = reader.ReadInteger();
            var command = (SmppCommand)reader.ReadInteger();
            reader.ReadInteger(); // skip status
            var sequence = (uint)reader.ReadInteger();
            var serviceType = reader.ReadString();
            var sourceTon = (TypeOfNumber)reader.ReadByte();
            var sourceNpi = (NumericPlanIndicator)reader.ReadByte();
            var sourceValue = reader.ReadString();
            var destTon = (TypeOfNumber)reader.ReadByte();
            var destNpi = (NumericPlanIndicator)reader.ReadByte();
            var destValue = reader.ReadString();
            var esmClass = reader.ReadByte();
            var protocolId = reader.ReadByte();
            var priorityFlag = reader.ReadByte();
            var scheduleDeliveryTime = reader.ReadString();
            var validityPeriod = reader.ReadString();
            var registeredDelivery = reader.ReadByte();
            var replaceIfPresent = reader.ReadByte() == 0x01;
            reader.ReadBytes(1); // skip unsupported functions
            var defaultMsgId = reader.ReadByte();
            var msgLength = reader.ReadByte();
            var shortMessage = reader.ReadBytes(msgLength);

            return new Submit
            {
                SequenceNumber = sequence,
                ServiceType = serviceType,
                Source = new Address(sourceTon, sourceNpi, sourceValue),
                Destination = new Address(destTon, destNpi, destValue),
                EsmClass = esmClass,
                ProtocolId = protocolId,
                PriorityFlag = (PriorityFlag)priorityFlag,
                ScheduleDeliveryTime = scheduleDeliveryTime,
                ValidityPeriod = validityPeriod,
                RegisteredDelivery = registeredDelivery,
                ReplaceIfPresent = replaceIfPresent,
                DefaultMessageId = defaultMsgId,
                ShortMessage = shortMessage
            };
        }
    }
}
