﻿using NUnit.Framework;
using NSmpp.Pdu;
using NSmpp.Serialization;

namespace NSmpp.Tests
{
    [TestFixture]
    public class EnquireLinkResponseSerializerTests
    {
        [Test]
        public void Deserializes_Pdu()
        {
            var data = new byte[] {
                0x00, 0x00, 0x00, 0x10,       // length
                0x80, 0x00, 0x00, 0x15,       // command
                0x00, 0x00, 0x00, 0x00,       // status
                0x00, 0x00, 0x00, 0x10,       // sequence
            };

            var serializer = new EnquireLinkResponseSerializer();
            var pdu = serializer.Deserialize(data);

            Assert.AreEqual(16, pdu.SequenceNumber);
        }

        [Test]
        public void Serializes_Pdu()
        {
            var expectedResult = new byte[] {
                0x00, 0x00, 0x00, 0x10,       // length
                0x80, 0x00, 0x00, 0x15,       // command
                0x00, 0x00, 0x00, 0x00,       // status
                0x00, 0x00, 0x00, 0x10,       // sequence
            };
            var pdu = new EnquireLinkResponse(SmppStatus.Ok, 16);

            var serializer = new EnquireLinkResponseSerializer();
            var result = serializer.Serialize(pdu);

            CollectionAssert.AreEqual(expectedResult, result);
        }
    }
}
