using FluentAssertions;
using NSmpp.Pdu;
using NSmpp.Serialization;

namespace NSmpp.Tests;

[TestFixture]
public class SubmitResponseSerializerTests
{
    [Test]
    public void Deserializes_Pdu()
    {
        byte[] data =
        [
            0x00, 0x00, 0x00, 0x31, // length
            0x80, 0x00, 0x00, 0x04, // command
            0x00, 0x00, 0x00, 0x00, // status
            0x00, 0x00, 0x00, 0x20, // sequence
            0x36, 0x62, 0x33, 0x31,
            0x36, 0x39, 0x65, 0x36,
            0x35, 0x65, 0x39, 0x38,
            0x34, 0x36, 0x36, 0x66,
            0x62, 0x32, 0x30, 0x64,
            0x31, 0x34, 0x39, 0x62,
            0x35, 0x36, 0x65, 0x37,
            0x34, 0x32, 0x36, 0x31,
            0x00                    // message id
        ];
        var serializer = new SubmitResponseSerializer();
        var pdu = serializer.Deserialize(data);

        pdu.Command.Should().Be(SmppCommand.SubmitResp);
        pdu.Status.Should().Be(SmppStatus.Ok);
        pdu.SequenceNumber.Should().Be(32);
        pdu.MessageId.Should().Be("6b3169e65e98466fb20d149b56e74261");
    }

    [Test]
    public void Deserializes_NonZero_Status_Without_Body()
    {
        byte[] data =
        [
            0x00, 0x00, 0x00, 0x10, // length
            0x80, 0x00, 0x00, 0x04, // command
            0x00, 0x00, 0x00, 0x14, // status
            0x00, 0x00, 0x00, 0x20  // sequence
        ];
        var serializer = new SubmitResponseSerializer();
        var pdu = serializer.Deserialize(data);

        pdu.Command.Should().Be(SmppCommand.SubmitResp);
        pdu.Status.Should().Be(SmppStatus.MessageQueueFull);
        pdu.SequenceNumber.Should().Be(32);
        pdu.MessageId.Should().BeNull();
    }

    [Test]
    public void Serializes_Pdu()
    {
        byte[] expectedResult =
        [
            0x00, 0x00, 0x00, 0x31, // length
            0x80, 0x00, 0x00, 0x04, // command
            0x00, 0x00, 0x00, 0x00, // status
            0x00, 0x00, 0x00, 0x20, // sequence
            0x36, 0x62, 0x33, 0x31,
            0x36, 0x39, 0x65, 0x36,
            0x35, 0x65, 0x39, 0x38,
            0x34, 0x36, 0x36, 0x66,
            0x62, 0x32, 0x30, 0x64,
            0x31, 0x34, 0x39, 0x62,
            0x35, 0x36, 0x65, 0x37,
            0x34, 0x32, 0x36, 0x31,
            0x00                    // message id
        ];
        var pdu = new SubmitResponse(
            SmppStatus.Ok,
            32,
            "6b3169e65e98466fb20d149b56e74261");
        var serializer = new SubmitResponseSerializer();

        var result = serializer.Serialize(pdu);

        result.Should().Equal(expectedResult);
    }

    [Test]
    public void Serializes_NonZero_Status_Without_Body()
    {
        byte[] expectedResult =
        [
            0x00, 0x00, 0x00, 0x10, // length
            0x80, 0x00, 0x00, 0x04, // command
            0x00, 0x00, 0x00, 0x14, // status
            0x00, 0x00, 0x00, 0x20  // sequence
        ];
        var serializer = new SubmitResponseSerializer();
        var pdu = new SubmitResponse(SmppStatus.MessageQueueFull, 32, null);

        var result = serializer.Serialize(pdu);

        result.Should().Equal(expectedResult);
    }
}