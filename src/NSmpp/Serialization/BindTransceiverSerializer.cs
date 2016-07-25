using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal class BindTransceiverSerializer : PduSerializer<BindTransceiver>
    {
        internal override byte[] Serialize(BindTransceiver pdu)
        {
            var writer = new PduWriter(pdu.Length);
            writer.WritePduHeader(pdu);
            writer.WriteString(pdu.SystemId);
            writer.WriteString(pdu.Password);
            writer.WriteString(pdu.SystemType);
            writer.WriteByte(pdu.InterfaceVersion);
            writer.WriteByte((byte)pdu.AddressTon);
            writer.WriteByte((byte)pdu.AddressNpi);
            writer.WriteString(pdu.AddressRange);

            return writer.GetBytes();
        }

        internal override BindTransceiver Deserialize(byte[] bytes)
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

            return new BindTransceiver(
                sequence,
                systemId,
                password,
                systemType,
                interfaceVersion,
                addressTon,
                addressNpi,
                addressRange);
        }
    }
}
