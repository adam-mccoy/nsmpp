using FluentAssertions;
using NSmpp.Pdu;
using NSmpp.Serialization;

namespace NSmpp.Tests;

[TestFixture]
public class UnbindSerializerTests
{
    [Test]
    public void Deserializes_Pdu()
    {
        byte[] data =
        [
            0x00, 0x00, 0x00, 0x10,       // length
            0x00, 0x00, 0x00, 0x06,       // command
            0x00, 0x00, 0x00, 0x00,       // status
            0x00, 0x00, 0x00, 0x10        // sequence
        ];

        var serializer = new UnbindSerializer();
        var pdu = serializer.Deserialize(data);

        pdu.Command.Should().Be(SmppCommand.Unbind);
        pdu.SequenceNumber.Should().Be(16);
    }

    [Test]
    public void Serializes_Pdu()
    {
        byte[] expectedResult =
        [
            0x00, 0x00, 0x00, 0x10,       // length
            0x00, 0x00, 0x00, 0x06,       // command
            0x00, 0x00, 0x00, 0x00,       // status
            0x00, 0x00, 0x00, 0x10        // sequence
        ];
        var pdu = new Unbind(16);

        var serializer = new UnbindSerializer();
        var result = serializer.Serialize(pdu);

        result.Should().Equal(expectedResult);
    }
}