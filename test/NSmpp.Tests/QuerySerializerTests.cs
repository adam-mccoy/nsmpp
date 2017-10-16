using NSmpp.Pdu;
using NSmpp.Serialization;
using NUnit.Framework;

namespace NSmpp.Tests
{
    [TestFixture]
    public class QuerySerializerTests
    {
        [Test]
        public void Deserializes_Pdu()
        {
            var data = new byte[]
            {
                0x00, 0x00, 0x00, 0x24,                   // length
                0x00, 0x00, 0x00, 0x03,                   // command
                0x00, 0x00, 0x00, 0x00,                   // status
                0x00, 0x00, 0x00, 0x10,                   // sequence
                0x73, 0x6f, 0x6d, 0x65, 0x69, 0x64, 0x00, // message_id
                0x02,                                     // source_ton
                0x08,                                     // source_npi
                0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, // source_address
                0x38, 0x39, 0x30, 0x00
            };

            var serializer = new QuerySerializer();
            var pdu = serializer.Deserialize(data);

            Assert.AreEqual(SmppCommand.Query, pdu.Command);
            Assert.AreEqual(16, pdu.SequenceNumber);
            Assert.AreEqual("someid", pdu.MessageId);
            Assert.AreEqual(TypeOfNumber.National, pdu.Source.Ton);
            Assert.AreEqual(NumericPlanIndicator.Internet, pdu.Source.Npi);
            Assert.AreEqual("1234567890", pdu.Source.Value);
        }

        [Test]
        public void Serializes_Pdu()
        {
            var expectedResult = new byte[]
            {
                0x00, 0x00, 0x00, 0x24,                   // length
                0x00, 0x00, 0x00, 0x03,                   // command
                0x00, 0x00, 0x00, 0x00,                   // status
                0x00, 0x00, 0x00, 0x10,                   // sequence
                0x73, 0x6f, 0x6d, 0x65, 0x69, 0x64, 0x00, // message_id
                0x02,                                     // source_ton
                0x08,                                     // source_npi
                0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, // source_address
                0x38, 0x39, 0x30, 0x00
            };

            var pdu = new Query
            {
                SequenceNumber = 16,
                MessageId = "someid",
                Source = new Address(TypeOfNumber.National, NumericPlanIndicator.Internet, "1234567890")
            };
            var serializer = new QuerySerializer();
            var result = serializer.Serialize(pdu);

            CollectionAssert.AreEqual(expectedResult, result);
        }
    }
}
