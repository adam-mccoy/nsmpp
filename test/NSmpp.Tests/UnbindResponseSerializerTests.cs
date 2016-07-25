using NUnit.Framework;
using NSmpp.Pdu;
using NSmpp.Serialization;

namespace NSmpp.Tests
{
    [TestFixture]
    public class UnbindResponseSerializerTests
    {
        [Test]
        public void Deserializes_Pdu()
        {
            var data = new byte[] {
                0x00, 0x00, 0x00, 0x10, // length
                0x80, 0x00, 0x00, 0x06, // command
                0x00, 0x00, 0x00, 0x00, // status
                0x00, 0x00, 0x00, 0x20  // sequence
            };

            var serializer = new UnbindResponseSerializer();
            var pdu = serializer.Deserialize(data);

            Assert.AreEqual(SmppCommand.UnbindResp, pdu.Command);
            Assert.AreEqual(SmppStatus.Ok, pdu.Status);
            Assert.AreEqual(32, pdu.SequenceNumber);
        }

        [Test]
        public void Serializes_Pdu()
        {
            var expectedResult = new byte[] {
                0x00, 0x00, 0x00, 0x10, // length
                0x80, 0x00, 0x00, 0x06, // command
                0x00, 0x00, 0x00, 0x00, // status
                0x00, 0x00, 0x00, 0x20, // sequence
            };
            var pdu = new UnbindResponse(
                SmppStatus.Ok,
                32);
            var serializer = new UnbindResponseSerializer();

            var result = serializer.Serialize(pdu);

            CollectionAssert.AreEqual(expectedResult, result);
        }
    }
}
