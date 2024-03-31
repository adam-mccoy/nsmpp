using FluentAssertions;
using NSmpp.Pdu;
using NSmpp.Serialization;

namespace NSmpp.Tests;

[TestFixture]
public class BindTransceiverTests
{
    [Test]
    public void Deserializes_Pdu()
    {
        byte[] data =
        [
            0x00, 0x00, 0x00, 0x22,       // length
            0x00, 0x00, 0x00, 0x09,       // command
            0x00, 0x00, 0x00, 0x00,       // status
            0x00, 0x00, 0x00, 0x10,       // sequence
            0x54, 0x45, 0x53, 0x54, 0x00, // system ID
            0x50, 0x41, 0x53, 0x53, 0x00, // password
            0x53, 0x4d, 0x53, 0x00,       // system type
            0x34,                         // interface version
            0x02,                         // address ton
            0x03,                         // address npi
            0x00                          // address range
        ];

        var serializer = new BindTransceiverSerializer();
        var pdu = serializer.Deserialize(data);

        pdu.Command.Should().Be(SmppCommand.BindTransceiver);
        pdu.SequenceNumber.Should().Be(16);
        pdu.SystemId.Should().Be("TEST");
        pdu.Password.Should().Be("PASS");
        pdu.SystemType.Should().Be("SMS");
        pdu.InterfaceVersion.Should().Be(52);
        pdu.AddressTon.Should().Be(TypeOfNumber.National);
        pdu.AddressNpi.Should().Be(NumericPlanIndicator.Telex);
        pdu.AddressRange.Should().BeNull();
    }

    [Test]
    public void Serializes_Pdu()
    {
        byte[] expectedResult =
        [
            0x00, 0x00, 0x00, 0x22,       // length
            0x00, 0x00, 0x00, 0x09,       // command
            0x00, 0x00, 0x00, 0x00,       // status
            0x00, 0x00, 0x00, 0x10,       // sequence
            0x54, 0x45, 0x53, 0x54, 0x00, // system ID
            0x50, 0x41, 0x53, 0x53, 0x00, // password
            0x53, 0x4d, 0x53, 0x00,       // system type
            0x34,                         // interface version
            0x02,                         // address ton
            0x03,                         // address npi
            0x00                          // address range
        ];
        var pdu = new BindTransceiver(
            16,
            "TEST",
            "PASS",
            "SMS",
            52,
            TypeOfNumber.National,
            NumericPlanIndicator.Telex,
            null);

        var serializer = new BindTransceiverSerializer();
        var result = serializer.Serialize(pdu);

        result.Should().Equal(expectedResult);
    }
}