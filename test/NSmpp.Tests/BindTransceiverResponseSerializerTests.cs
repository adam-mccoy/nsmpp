using NUnit.Framework;
using NSmpp.Pdu;
using NSmpp.Serialization;

namespace NSmpp.Tests
{
    [TestFixture]
    public class BindTransceiverResponseSerializerTests
    {
        [Test]
        public void Deserializes_Pdu()
        {
            var data = new byte[] {
                0x00, 0x00, 0x00, 0x17, // length
                0x80, 0x00, 0x00, 0x09, // command
                0x00, 0x00, 0x00, 0x00, // status
                0x00, 0x00, 0x00, 0x20, // sequence
                0x53, 0x79, 0x73, 0x74, 0x65, 0x6d, 0x00 // system ID
            };

            var serializer = new BindTransceiverResponseSerializer();
            var pdu = serializer.Deserialize(data);

            Assert.AreEqual(SmppCommand.BindTransceiverResp, pdu.Command);
            Assert.AreEqual(SmppStatus.Ok, pdu.Status);
            Assert.AreEqual(32, pdu.SequenceNumber);
            Assert.AreEqual("System", pdu.SystemId);
        }

        [Test]
        public void Serializes_Pdu()
        {
            var expectedResult = new byte[] {
                0x00, 0x00, 0x00, 0x17, // length
                0x80, 0x00, 0x00, 0x09, // command
                0x00, 0x00, 0x00, 0x00, // status
                0x00, 0x00, 0x00, 0x20, // sequence
                0x53, 0x79, 0x73, 0x74, 0x65, 0x6d, 0x00 // system ID
            };
            var pdu = new BindTransceiverResponse
            {
                Status = SmppStatus.Ok,
                SequenceNumber = 32,
                SystemId = "System"
            };
            var serializer = new BindTransceiverResponseSerializer();

            var result = serializer.Serialize(pdu);

            CollectionAssert.AreEqual(expectedResult, result);
        }
    }
}
