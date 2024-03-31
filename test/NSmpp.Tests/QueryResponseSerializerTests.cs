using FluentAssertions;
using NSmpp.Pdu;
using NSmpp.Serialization;

namespace NSmpp.Tests;

[TestFixture]
public class QueryResponseSerializerTests
{
    [Test]
    public void Deserializes_Pdu()
    {
        byte[] data =
        [
            0x00, 0x00, 0x00, 0x2a,                   // length
            0x80, 0x00, 0x00, 0x03,                   // command
            0x00, 0x00, 0x00, 0x00,                   // status
            0x00, 0x00, 0x00, 0x10,                   // sequence
            0x73, 0x6f, 0x6d, 0x65, 0x69, 0x64, 0x00, // message_id
            0x31, 0x36, 0x30, 0x38, 0x31, 0x38, 0x31, // final_date
            0x36, 0x33, 0x34, 0x35, 0x36, 0x34, 0x34,
            0x30, 0x2b, 0x00,
            0x05,                                     // message_state
            0xff                                      // error_code
        ];

        var serializer = new QueryResponseSerializer();
        var pdu = serializer.Deserialize(data);

        pdu.Command.Should().Be(SmppCommand.QueryResp);
        pdu.SequenceNumber.Should().Be(16);
        pdu.MessageId.Should().Be("someid");
        pdu.FinalDate.Should().Be(new DateTimeOffset(2016, 8, 18, 16, 34, 56, 400, TimeSpan.FromHours(10)));
        pdu.MessageState.Should().Be(MessageState.Undeliverable);
        pdu.ErrorCode.Should().Be(255);
    }

    [Test]
    public void Serializes_Pdu()
    {
        byte[] expectedResult =
        [
            0x00, 0x00, 0x00, 0x2a,                   // length
            0x80, 0x00, 0x00, 0x03,                   // command
            0x00, 0x00, 0x00, 0x00,                   // status
            0x00, 0x00, 0x00, 0x10,                   // sequence
            0x73, 0x6f, 0x6d, 0x65, 0x69, 0x64, 0x00, // message_id
            0x31, 0x36, 0x30, 0x38, 0x31, 0x38, 0x31, // final_date
            0x36, 0x33, 0x34, 0x35, 0x36, 0x34, 0x34,
            0x30, 0x2b, 0x00,
            0x05,                                     // message_state
            0xff                                      // error_code
        ];

        var pdu = new QueryResponse(
            SmppStatus.Ok,
            16,
            "someid",
            new DateTimeOffset(2016, 8, 18, 16, 34, 56, 400, TimeSpan.FromHours(10)),
            MessageState.Undeliverable,
            255);
        var serializer = new QueryResponseSerializer();
        var result = serializer.Serialize(pdu);

        result.Should().Equal(expectedResult);
    }
}