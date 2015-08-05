using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal interface IPduSerializer
    {
        byte[] Serialize(PduBase pdu);
        PduBase Deserialize(byte[] bytes);
    }
}
