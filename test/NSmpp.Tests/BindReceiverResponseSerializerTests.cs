using FluentAssertions;
using NSmpp.Pdu;
using NSmpp.Serialization;

namespace NSmpp.Tests;

[TestFixture]
public class BindReceiverResponseSerializerTests
{
    [Test]
    public void Deserializes_Pdu()
    {
        byte[] data =
        [
            0x00, 0x00, 0x00, 0x17, // length
            0x80, 0x00, 0x00, 0x01, // command
            0x00, 0x00, 0x00, 0x00, // status
            0x00, 0x00, 0x00, 0x20, // sequence
            0x53, 0x79, 0x73, 0x74, 0x65, 0x6d, 0x00 // system ID
        ];

        var serializer = new BindReceiverResponseSerializer();
        var pdu = serializer.Deserialize(data);

        pdu.Command.Should().Be(SmppCommand.BindReceiverResp);
        pdu.Status.Should().Be(SmppStatus.Ok);
        pdu.SequenceNumber.Should().Be(32);
        pdu.SystemId.Should().Be("System");
    }

    [Test]
    public void Serializes_Pdu()
    {
        byte[] expectedResult =
        [
            0x00, 0x00, 0x00, 0x17, // length
            0x80, 0x00, 0x00, 0x01, // command
            0x00, 0x00, 0x00, 0x00, // status
            0x00, 0x00, 0x00, 0x20, // sequence
            0x53, 0x79, 0x73, 0x74, 0x65, 0x6d, 0x00 // system ID
        ];
        var pdu = new BindReceiverResponse(
            SmppStatus.Ok,
            32,
            "System");
        var serializer = new BindReceiverResponseSerializer();

        var result = serializer.Serialize(pdu);

        result.Should().Equal(expectedResult);
    }
}