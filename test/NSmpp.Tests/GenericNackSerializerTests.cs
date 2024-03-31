using FluentAssertions;
using NSmpp.Pdu;
using NSmpp.Serialization;

namespace NSmpp.Tests;

[TestFixture]
public class GenericNackSerializerTests
{
    [TestCase(0x01)]
    [TestCase(0x02)]
    [TestCase(0x03)]
    public void Deserializes_Pdu(byte statusByte)
    {
        byte[] data =
        [
            0x00, 0x00, 0x00, 0x10, // length
            0x80, 0x00, 0x00, 0x00, // command
            0x00, 0x00, 0x00, statusByte, // status
            0x00, 0x00, 0x00, 0x20 // sequence
        ];

        var serializer = new GenericNackSerializer();
        var pdu = serializer.Deserialize(data);

        pdu.Command.Should().Be(SmppCommand.GenericNack);
        pdu.SequenceNumber.Should().Be(32);
        pdu.Status.Should().Be((SmppStatus)statusByte);
    }

    [TestCase(0x01)]
    [TestCase(0x02)]
    [TestCase(0x03)]
    public void Serializes_Pdu(byte statusByte)
    {
        byte[] expectedResult =
        [
            0x00, 0x00, 0x00, 0x10, // length
            0x80, 0x00, 0x00, 0x00, // command
            0x00, 0x00, 0x00, statusByte, // status
            0x00, 0x00, 0x00, 0x20 // sequence
        ];

        var pdu = new GenericNack((SmppStatus)statusByte, 32);
        var serializer = new GenericNackSerializer();

        var result = serializer.Serialize(pdu);

        result.Should().Equal(expectedResult);
    }
}