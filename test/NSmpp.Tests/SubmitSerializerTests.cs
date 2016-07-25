using NUnit.Framework;
using NSmpp.Pdu;
using NSmpp.Serialization;

namespace NSmpp.Tests
{
    [TestFixture]
    public class SubmitSerializerTests
    {
        [Test]
        public void Deserializes_Pdu()
        {
            var data = new byte[] {
                0x00, 0x00, 0x00, 0x4c,       // length
                0x00, 0x00, 0x00, 0x04,       // command
                0x00, 0x00, 0x00, 0x00,       // status
                0x00, 0x00, 0x00, 0x10,       // sequence
                0x00,                         // service type
                0x02,                         // source ton
                0x05,                         // source npi
                0x31, 0x32, 0x33, 0x34, 0x35,
                0x36, 0x37, 0x38, 0x39, 0x30,
                0x00,                         // source address
                0x03,                         // dest ton
                0x08,                         // dest npi
                0x39, 0x38, 0x37, 0x36, 0x35,
                0x34, 0x33, 0x32, 0x31, 0x30,
                0x00,                         // dest address
                0x00,                         // esm class
                0x00,                         // protocol id
                0x00,                         // priority flag
                0x00,                         // scheduled delivery time
                0x00,                         // validity period
                0x00,                         // registered delivery
                0x00,                         // replace if present
                0x00,                         // data coding
                0x00,                         // sm default msg id
                0x17,
                0x54, 0x68, 0x69, 0x73, 0x20,
                0x69, 0x73, 0x20, 0x61, 0x20,
                0x74, 0x65, 0x73, 0x74, 0x20,
                0x6d, 0x65, 0x73, 0x73, 0x61,
                0x67, 0x65, 0x2e              // short message
            };

            var serializer = new SubmitSerializer();
            var pdu = serializer.Deserialize(data);

            Assert.AreEqual(SmppCommand.Submit, pdu.Command);
            Assert.AreEqual(16, pdu.SequenceNumber);
            Assert.IsNull(pdu.ServiceType);
            Assert.AreEqual(TypeOfNumber.National, pdu.SourceTon);
            Assert.AreEqual(NumericPlanIndicator.National, pdu.SourceNpi);
            Assert.AreEqual("1234567890", pdu.SourceAddress);
            Assert.AreEqual(TypeOfNumber.NetworkSpecific, pdu.DestTon);
            Assert.AreEqual(NumericPlanIndicator.Internet, pdu.DestNpi);
            Assert.AreEqual("9876543210", pdu.DestAddress);
            Assert.AreEqual(0, pdu.EsmClass);
            Assert.AreEqual("This is a test message.", pdu.ShortMessage);
        }

        [Test]
        public void Serializes_Pdu()
        {
            var expectedResult = new byte[] {
                0x00, 0x00, 0x00, 0x4c,       // length
                0x00, 0x00, 0x00, 0x04,       // command
                0x00, 0x00, 0x00, 0x00,       // status
                0x00, 0x00, 0x00, 0x10,       // sequence
                0x00,                         // service type
                0x02,                         // source ton
                0x05,                         // source npi
                0x31, 0x32, 0x33, 0x34, 0x35,
                0x36, 0x37, 0x38, 0x39, 0x30,
                0x00,                         // source address
                0x03,                         // dest ton
                0x08,                         // dest npi
                0x39, 0x38, 0x37, 0x36, 0x35,
                0x34, 0x33, 0x32, 0x31, 0x30,
                0x00,                         // dest address
                0x00,                         // esm class
                0x00,                         // protocol id
                0x00,                         // priority flag
                0x00,                         // scheduled delivery time
                0x00,                         // validity period
                0x00,                         // registered delivery
                0x00,                         // replace if present
                0x00,                         // data coding
                0x00,                         // sm default msg id
                0x17,
                0x54, 0x68, 0x69, 0x73, 0x20,
                0x69, 0x73, 0x20, 0x61, 0x20,
                0x74, 0x65, 0x73, 0x74, 0x20,
                0x6d, 0x65, 0x73, 0x73, 0x61,
                0x67, 0x65, 0x2e              // short message
            };
            var pdu = new Submit(
                16,
                null,
                TypeOfNumber.National,
                NumericPlanIndicator.National,
                "1234567890",
                TypeOfNumber.NetworkSpecific,
                NumericPlanIndicator.Internet,
                "9876543210",
                0, 0, 0,
                "This is a test message.");

            var serializer = new SubmitSerializer();
            var result = serializer.Serialize(pdu);

            CollectionAssert.AreEqual(expectedResult, result);
        }
    }
}
