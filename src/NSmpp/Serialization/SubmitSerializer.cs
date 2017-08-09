using System.Text;
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
            writer.WriteByte(0x00); // schedule delivery time
            writer.WriteByte(0x00); // validity period
            writer.WriteByte(0x00); // registered delivery
            writer.WriteByte(0x00); // replace if present
            writer.WriteByte(0x00); // data coding
            writer.WriteByte(0x00); // sm default msg id
            writer.WriteByte((byte)pdu.ShortMessage.Length);
            writer.WriteBytes(Encoding.ASCII.GetBytes(pdu.ShortMessage));

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
            reader.ReadBytes(6); // skip unsupported functions
            var msgLength = reader.ReadByte();
            var shortMessage = reader.ReadBytes(msgLength);

            return new Submit(
                sequence,
                serviceType,
                new Address(sourceTon, sourceNpi, sourceValue),
                new Address(destTon, destNpi, destValue),
                esmClass,
                protocolId,
                priorityFlag,
                Encoding.ASCII.GetString(shortMessage));
        }
    }
}
