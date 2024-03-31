using FluentAssertions;
using NSmpp.Pdu;
using NSmpp.Serialization;

namespace NSmpp.Tests;

[TestFixture]
public class EnquireLinkSerializerTests
{
    [Test]
    public void Deserializes_Pdu()
    {
        byte[] data =
        [
            0x00, 0x00, 0x00, 0x10,       // length
            0x00, 0x00, 0x00, 0x15,       // command
            0x00, 0x00, 0x00, 0x00,       // status
            0x00, 0x00, 0x00, 0x10        // sequence
        ];

        var serializer = new EnquireLinkSerializer();
        var pdu = serializer.Deserialize(data);

        pdu.SequenceNumber.Should().Be(16);
    }

    [Test]
    public void Serializes_Pdu()
    {
        byte[] expectedResult =
        [
            0x00, 0x00, 0x00, 0x10,       // length
            0x00, 0x00, 0x00, 0x15,       // command
            0x00, 0x00, 0x00, 0x00,       // status
            0x00, 0x00, 0x00, 0x10        // sequence
        ];
        var pdu = new EnquireLink(16);

        var serializer = new EnquireLinkSerializer();
        var result = serializer.Serialize(pdu);

        result.Should().Equal(expectedResult);
    }
}