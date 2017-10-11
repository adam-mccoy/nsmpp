using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal class BindTransmitterSerializer : PduSerializer<BindTransmitter>
    {
        internal override byte[] Serialize(BindTransmitter pdu)
        {
            var writer = new PduWriter();
            writer.WritePduHeader(pdu);
            writer.WriteString(pdu.SystemId);
            writer.WriteString(pdu.Password);
            writer.WriteString(pdu.SystemType);
            writer.WriteByte(pdu.InterfaceVersion);
            writer.WriteByte((byte)pdu.AddressTon);
            writer.WriteByte((byte)pdu.AddressNpi);
            writer.WriteString(pdu.AddressRange);

            var bytes = writer.GetBytes();
            PduWriter.WriteInteger(bytes, 0, bytes.Length);
            return bytes;
        }

        internal override BindTransmitter Deserialize(byte[] bytes)
        {
            var reader = new PduReader(bytes);
            int length = reader.ReadInteger();
            var command = (SmppCommand)reader.ReadInteger();
            reader.ReadInteger(); // skip status
            var sequence = (uint)reader.ReadInteger();
            var systemId = reader.ReadString();
            var password = reader.ReadString();
            var systemType = reader.ReadString();
            var interfaceVersion = reader.ReadByte();
            var addressTon = (TypeOfNumber)reader.ReadByte();
            var addressNpi = (NumericPlanIndicator)reader.ReadByte();
            var addressRange = reader.ReadString();

            return new BindTransmitter
            {
                SequenceNumber = sequence,
                SystemId = systemId,
                Password = password,
                SystemType = systemType,
                InterfaceVersion = interfaceVersion,
                AddressTon = addressTon,
                AddressNpi = addressNpi,
                AddressRange = addressRange
            };
        }
    }
}
