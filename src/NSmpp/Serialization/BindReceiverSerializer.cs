using NSmpp.Pdu;

namespace NSmpp.Serialization;

internal class BindReceiverSerializer : PduSerializer<BindReceiver>
{
    internal override byte[] Serialize(BindReceiver pdu)
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
        PduWriter.WriteInt32(bytes, 0, bytes.Length);
        return bytes;
    }

    internal override BindReceiver Deserialize(byte[] bytes)
    {
        var reader = new PduReader(bytes);
        int length = reader.ReadInt32();
        var command = (SmppCommand)reader.ReadInt32();
        reader.ReadInt32(); // skip status
        var sequence = (uint)reader.ReadInt32();
        var systemId = reader.ReadString();
        var password = reader.ReadString();
        var systemType = reader.ReadString();
        var interfaceVersion = reader.ReadByte();
        var addressTon = (TypeOfNumber)reader.ReadByte();
        var addressNpi = (NumericPlanIndicator)reader.ReadByte();
        var addressRange = reader.ReadString();

        return new BindReceiver(
            systemId,
            password,
            systemType,
            interfaceVersion,
            addressTon,
            addressNpi,
            addressRange);
    }
}