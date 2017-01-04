using NSmpp.Pdu;
using NSmpp.Serialization;
using NUnit.Framework;

namespace NSmpp.Tests
{
    [TestFixture]
    public class DeliverSerializerTests
    {
        #region Test Data
        private static byte[] EncodedPdu = new byte[]
        {
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
        };
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

            CollectionAssert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Deserializes_Pdu()
        {
            var serializer = new DeliverSerializer();
            var pdu = serializer.Deserialize(EncodedPdu);

            Assert.AreEqual(16, pdu.SequenceNumber);
            Assert.AreEqual("Test", pdu.ServiceType);
            Assert.AreEqual(new Address(TypeOfNumber.National, NumericPlanIndicator.National, "1234567890"), pdu.Source);
            Assert.AreEqual(new Address(TypeOfNumber.NetworkSpecific, NumericPlanIndicator.Internet, "9876543210"), pdu.Destination);
            Assert.AreEqual(MessagingMode.Default, pdu.Mode);
            Assert.AreEqual(MessageType.DeliveryAcknowledgement, pdu.Type);
            Assert.AreEqual(NetworkSpecificFeatures.None, pdu.Features);
            Assert.AreEqual(0x12, pdu.ProtocolId);
            Assert.AreEqual(PriorityFlag.Urgent, pdu.Priority);
            Assert.AreEqual(0x00, pdu.RegisteredDelivery);
            Assert.AreEqual(0x03, pdu.DataCoding);
            Assert.AreEqual(25, pdu.Length);
            Assert.AreEqual("This is a short message.", pdu.ShortMessage);
        }
    }
}
