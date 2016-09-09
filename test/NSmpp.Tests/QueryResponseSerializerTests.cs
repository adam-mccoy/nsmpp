using System;
using NSmpp.Pdu;
using NSmpp.Serialization;
using NUnit.Framework;

namespace NSmpp.Tests
{
    [TestFixture]
    public class QueryResponseSerializerTests
    {
        [Test]
        public void Deserializes_Pdu()
        {
            var data = new byte[]
            {
                0x00, 0x00, 0x00, 0x25,                   // length
                0x80, 0x00, 0x00, 0x03,                   // command
                0x00, 0x00, 0x00, 0x00,                   // status
                0x00, 0x00, 0x00, 0x10,                   // sequence
                0x73, 0x6f, 0x6d, 0x65, 0x69, 0x64, 0x00, // message_id
                0x31, 0x36, 0x30, 0x38, 0x31, 0x38, 0x31, // final_date
                0x36, 0x33, 0x34, 0x35, 0x36,
                0x05,                                     // message_state
                0xff                                      // error_code
            };

            var serializer = new QueryResponseSerializer();
            var pdu = serializer.Deserialize(data);

            Assert.AreEqual(SmppCommand.QueryResp, pdu.Command);
            Assert.AreEqual(16, pdu.SequenceNumber);
            Assert.AreEqual("someid", pdu.MessageId);
            Assert.AreEqual(new DateTime(2016, 8, 18, 16, 34, 56, DateTimeKind.Utc), pdu.FinalDate);
            Assert.AreEqual(MessageState.Undeliverable, pdu.MessageState);
            Assert.AreEqual(255, pdu.ErrorCode);
        }

        [Test]
        public void Serializes_Pdu()
        {
            var expectedResult = new byte[]
            {
                0x00, 0x00, 0x00, 0x25,                   // length
                0x80, 0x00, 0x00, 0x03,                   // command
                0x00, 0x00, 0x00, 0x00,                   // status
                0x00, 0x00, 0x00, 0x10,                   // sequence
                0x73, 0x6f, 0x6d, 0x65, 0x69, 0x64, 0x00, // message_id
                0x31, 0x36, 0x30, 0x38, 0x31, 0x38, 0x31, // final_date
                0x36, 0x33, 0x34, 0x35, 0x36,
                0x05,                                     // message_state
                0xff                                      // error_code
            };

            var pdu = new QueryResponse(
                SmppStatus.Ok,
                16,
                "someid",
                new DateTime(2016, 8, 18, 16, 34, 56, DateTimeKind.Utc),
                MessageState.Undeliverable,
                255);
            var serializer = new QueryResponseSerializer();
            var result = serializer.Serialize(pdu);

            CollectionAssert.AreEqual(expectedResult, result);
        }
    }
}
