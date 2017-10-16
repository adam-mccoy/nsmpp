using NUnit.Framework;
using NSmpp.Pdu;
using NSmpp.Serialization;

namespace NSmpp.Tests
{
    [TestFixture]
    public class DeliverResponseSerializerTests
    {
        #region Test Data
        private static readonly byte[] EncodedPdu = new byte[]
        {
            0x00, 0x00, 0x00, 0x11, // length
            0x80, 0x00, 0x00, 0x05, // command
            0x00, 0x00, 0x00, 0x00, // status
            0x00, 0x00, 0x00, 0x10, // sequence
            0x00                    // message id
        };
        #endregion

        [Test]
        public void Serializes_Pdu()
        {
            var expectedResult = EncodedPdu;

            var pdu = new DeliverResponse
            {
                Status = SmppStatus.Ok,
                SequenceNumber = 16
            };
            var serializer = new DeliverResponseSerializer();
            var result = serializer.Serialize(pdu);

            CollectionAssert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Deserializes_Pdu()
        {
            var serializer = new DeliverResponseSerializer();
            var pdu = serializer.Deserialize(EncodedPdu);

            Assert.AreEqual(16, pdu.SequenceNumber);
            Assert.AreEqual(SmppStatus.Ok, pdu.Status);
        }
    }
}
