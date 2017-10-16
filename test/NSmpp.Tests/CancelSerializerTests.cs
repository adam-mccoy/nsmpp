using NUnit.Framework;
using NSmpp.Serialization;
using NSmpp.Pdu;

namespace NSmpp.Tests
{
    [TestFixture]
    public class CancelSerializerTests
    {
        [Test]
        public void Deserializes_Pdu()
        {
            var data = new byte[]
            {
                0x00, 0x00, 0x00, 0x38,                   // length
                0x00, 0x00, 0x00, 0x08,                   // command
                0x00, 0x00, 0x00, 0x00,                   // status
                0x00, 0x00, 0x00, 0x20,                   // sequence
                0x42, 0x61, 0x6e, 0x61, 0x6e, 0x61, 0x00, // service type
                0x39, 0x39, 0x39, 0x38, 0x38, 0x38, 0x00, // message ID
                0x00, 0x00,                               // source ton/npi
                0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30, 0x00, // source address
                0x00, 0x00,                               // dest ton/npi
                0x39, 0x38, 0x37, 0x36, 0x35, 0x34, 0x33, 0x32, 0x31, 0x30, 0x00 // dest address
            };

            var serializer = new CancelSerializer();
            var pdu = serializer.Deserialize(data);

            Assert.AreEqual(SmppCommand.Cancel, pdu.Command);
            Assert.AreEqual(32, pdu.SequenceNumber);
            Assert.AreEqual("Banana", pdu.ServiceType);
            Assert.AreEqual("999888", pdu.MessageId);
            Assert.AreEqual(new Address("1234567890"), pdu.Source);
            Assert.AreEqual(new Address("9876543210"), pdu.Destination);
        }

        [Test]
        public void Serializes_Pdu()
        {
            var expectedResult = new byte[]
            {
                0x00, 0x00, 0x00, 0x38,                   // length
                0x00, 0x00, 0x00, 0x08,                   // command
                0x00, 0x00, 0x00, 0x00,                   // status
                0x00, 0x00, 0x00, 0x20,                   // sequence
                0x42, 0x61, 0x6e, 0x61, 0x6e, 0x61, 0x00, // service type
                0x39, 0x39, 0x39, 0x38, 0x38, 0x38, 0x00, // message ID
                0x00, 0x00,                               // source ton/npi
                0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30, 0x00, // source address
                0x00, 0x00,                               // dest ton/npi
                0x39, 0x38, 0x37, 0x36, 0x35, 0x34, 0x33, 0x32, 0x31, 0x30, 0x00 // dest address
            };

            var pdu = new Cancel
            {
                SequenceNumber = 32,
                ServiceType = "Banana",
                MessageId = "999888",
                Source = "1234567890",
                Destination = "9876543210"
            };

            var serializer = new CancelSerializer();
            var result = serializer.Serialize(pdu);

            CollectionAssert.AreEqual(expectedResult, result);
        }
    }
}
