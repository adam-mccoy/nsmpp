using NUnit.Framework;
using NSmpp.Pdu;
using NSmpp.Serialization;

namespace NSmpp.Tests
{
    [TestFixture]
    public class BindTransmitterResponseSerializerTests
    {
        [Test]
        public void Deserializes_Pdu()
        {
            var data = new byte[] {
                0x00, 0x00, 0x00, 0x17, // length
                0x80, 0x00, 0x00, 0x02, // command
                0x00, 0x00, 0x00, 0x00, // status
                0x00, 0x00, 0x00, 0x20, // sequence
                0x53, 0x79, 0x73, 0x74, 0x65, 0x6d, 0x00 // system ID
            };

            var serializer = new BindTransmitterResponseSerializer();
            var pdu = serializer.Deserialize(data);

            Assert.AreEqual(23, pdu.Length);
            Assert.AreEqual(SmppCommand.BindTransmitterResp, pdu.Command);
            Assert.AreEqual(SmppStatus.Ok, pdu.Status);
            Assert.AreEqual(32, pdu.SequenceNumber);
            Assert.AreEqual("System", pdu.SystemId);
        }

        [Test]
        public void Serializes_Pdu()
        {
            var expectedResult = new byte[] {
                0x00, 0x00, 0x00, 0x17, // length
                0x80, 0x00, 0x00, 0x02, // command
                0x00, 0x00, 0x00, 0x00, // status
                0x00, 0x00, 0x00, 0x20, // sequence
                0x53, 0x79, 0x73, 0x74, 0x65, 0x6d, 0x00 // system ID
            };
            var pdu = new BindTransmitterResponse(
                SmppStatus.Ok,
                32,
                "System");
            var serializer = new BindTransmitterResponseSerializer();

            var result = serializer.Serialize(pdu);

            CollectionAssert.AreEqual(expectedResult, result);
        }
    }
}
