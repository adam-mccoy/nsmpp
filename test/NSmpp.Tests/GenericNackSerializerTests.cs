using NSmpp.Pdu;
using NSmpp.Serialization;
using NUnit.Framework;

namespace NSmpp.Tests
{
    [TestFixture]
    public class GenericNackSerializerTests
    {
        [TestCase(0x01)]
        [TestCase(0x02)]
        [TestCase(0x03)]
        public void Deserializes_Pdu(byte statusByte)
        {
            var data = new byte[]
            {
                0x00, 0x00, 0x00, 0x10, // length
                0x80, 0x00, 0x00, 0x00, // command
                0x00, 0x00, 0x00, statusByte, // status
                0x00, 0x00, 0x00, 0x20, // sequence
            };

            var serializer = new GenericNackSerializer();
            var pdu = serializer.Deserialize(data);

            Assert.AreEqual(SmppCommand.GenericNack, pdu.Command);
            Assert.AreEqual(32, pdu.SequenceNumber);
            Assert.AreEqual((SmppStatus)statusByte, pdu.Status);
        }

        [TestCase(0x01)]
        [TestCase(0x02)]
        [TestCase(0x03)]
        public void Serializes_Pdu(byte statusByte)
        {
            var expectedResult = new byte[]
            {
                0x00, 0x00, 0x00, 0x10, // length
                0x80, 0x00, 0x00, 0x00, // command
                0x00, 0x00, 0x00, statusByte, // status
                0x00, 0x00, 0x00, 0x20, // sequence
            };

            var pdu = new GenericNack
            {
                Status = (SmppStatus)statusByte,
                SequenceNumber = 32
            };
            var serializer = new GenericNackSerializer();

            var result = serializer.Serialize(pdu);

            CollectionAssert.AreEqual(expectedResult, result);
        }
    }
}
