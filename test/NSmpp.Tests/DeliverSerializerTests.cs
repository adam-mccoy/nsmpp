using FluentAssertions;
using NSmpp.Pdu;
using NSmpp.Serialization;

namespace NSmpp.Tests;

[TestFixture]
public class DeliverSerializerTests
{
    #region Test Data
    private static readonly byte[] EncodedPdu =
    [
        0x00, 0x00, 0x00, 0x52, // length
        0x00, 0x00, 0x00, 0x05, // command
        0x00, 0x00, 0x00, 0x00, // status
        0x00, 0x00, 0x00, 0x10, // sequence
        0x54, 0x65, 0x73, 0x74, 0x00, // service type
        0x02, // source ton
        0x05, // source npi
        0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30, 0x00, // source address
        0x03, // dest ton
        0x08, // dest npi
        0x39, 0x38, 0x37, 0x36, 0x35, 0x34, 0x33, 0x32, 0x31, 0x30, 0x00, // dest address
        0x08, // esm class
        0x12, // protocol id
        0x02, // priority flag
        0x00, 0x00,
        0x00, // registered delivery
        0x00,
        0x03, // data coding
        0x00,
        0x19, // length
        0x54, 0x68, 0x69, 0x73, 0x20, 0x69, 0x73, 0x20, 0x61, 0x20, 0x73,
        0x68, 0x6f, 0x72, 0x74, 0x20, 0x6d, 0x65, 0x73, 0x73, 0x61, 0x67,
        0x65, 0x2e, 0x00 // message
    ];
    #endregion

    [Test]
    public void Serializes_Pdu()
    {
        var expectedResult = EncodedPdu;

        var pdu = new Deliver(
            16,
            "Test",
            new Address(TypeOfNumber.National, NumericPlanIndicator.National, "1234567890"),
            new Address(TypeOfNumber.NetworkSpecific, NumericPlanIndicator.Internet, "9876543210"),
            MessagingMode.Default,
            MessageType.DeliveryAcknowledgement,
            NetworkSpecificFeatures.None,
            0x12,
            PriorityFlag.Urgent,
            0x00,
            0x03,
            25,
            "This is a short message.");
        var serializer = new DeliverSerializer();
        var result = serializer.Serialize(pdu);

        result.Should().Equal(expectedResult);
    }

    [Test]
    public void Deserializes_Pdu()
    {
        var serializer = new DeliverSerializer();
        var pdu = serializer.Deserialize(EncodedPdu);

        pdu.SequenceNumber.Should().Be(16);
        pdu.ServiceType.Should().Be("Test");
        pdu.Source.Should().Be(new Address(TypeOfNumber.National, NumericPlanIndicator.National, "1234567890"));
        pdu.Destination.Should().Be(new Address(TypeOfNumber.NetworkSpecific, NumericPlanIndicator.Internet, "9876543210"));
        pdu.Mode.Should().Be(MessagingMode.Default);
        pdu.Type.Should().Be(MessageType.DeliveryAcknowledgement);
        pdu.Features.Should().Be(NetworkSpecificFeatures.None);
        pdu.ProtocolId.Should().Be(0x12);
        pdu.Priority.Should().Be(PriorityFlag.Urgent);
        pdu.RegisteredDelivery.Should().Be(0x00);
        pdu.DataCoding.Should().Be(0x03);
        pdu.Length.Should().Be(25);
        pdu.ShortMessage.Should().Be("This is a short message.");
    }
}