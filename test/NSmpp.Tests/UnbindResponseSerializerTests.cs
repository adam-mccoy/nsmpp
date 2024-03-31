using FluentAssertions;
using NSmpp.Pdu;
using NSmpp.Serialization;

namespace NSmpp.Tests;

[TestFixture]
public class UnbindResponseSerializerTests
{
    [Test]
    public void Deserializes_Pdu()
    {
        byte[] data =
        [
            0x00, 0x00, 0x00, 0x10, // length
            0x80, 0x00, 0x00, 0x06, // command
            0x00, 0x00, 0x00, 0x00, // status
            0x00, 0x00, 0x00, 0x20  // sequence
        ];

        var serializer = new UnbindResponseSerializer();
        var pdu = serializer.Deserialize(data);

        pdu.Command.Should().Be(SmppCommand.UnbindResp);
        pdu.Status.Should().Be(SmppStatus.Ok);
        pdu.SequenceNumber.Should().Be(32);
    }

    [Test]
    public void Serializes_Pdu()
    {
        byte[] expectedResult =
        [
            0x00, 0x00, 0x00, 0x10, // length
            0x80, 0x00, 0x00, 0x06, // command
            0x00, 0x00, 0x00, 0x00, // status
            0x00, 0x00, 0x00, 0x20  // sequence
        ];
        var pdu = new UnbindResponse(
            SmppStatus.Ok,
            32);
        var serializer = new UnbindResponseSerializer();

        var result = serializer.Serialize(pdu);

        result.Should().Equal(expectedResult);
    }
}