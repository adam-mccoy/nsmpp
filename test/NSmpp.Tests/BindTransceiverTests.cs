﻿using NUnit.Framework;
using NSmpp.Pdu;
using NSmpp.Serialization;

namespace NSmpp.Tests
{
    [TestFixture]
    public class BindTransceiverTests
    {
        [Test]
        public void Deserializes_Pdu()
        {
            var data = new byte[] {
                0x00, 0x00, 0x00, 0x22,       // length
                0x00, 0x00, 0x00, 0x09,       // command
                0x00, 0x00, 0x00, 0x00,       // status
                0x00, 0x00, 0x00, 0x10,       // sequence
                0x54, 0x45, 0x53, 0x54, 0x00, // system ID
                0x50, 0x41, 0x53, 0x53, 0x00, // password
                0x53, 0x4d, 0x53, 0x00,       // system type
                0x34,                         // interface version
                0x02,                         // address ton
                0x03,                         // address npi
                0x00,                         // address range
            };

            var serializer = new BindTransceiverSerializer();
            var pdu = serializer.Deserialize(data);

            Assert.AreEqual(SmppCommand.BindTransceiver, pdu.Command);
            Assert.AreEqual(16, pdu.SequenceNumber);
            Assert.AreEqual("TEST", pdu.SystemId);
            Assert.AreEqual("PASS", pdu.Password);
            Assert.AreEqual("SMS", pdu.SystemType);
            Assert.AreEqual(52, pdu.InterfaceVersion);
            Assert.AreEqual(TypeOfNumber.National, pdu.AddressTon);
            Assert.AreEqual(NumericPlanIndicator.Telex, pdu.AddressNpi);
            Assert.IsNull(pdu.AddressRange);
        }

        [Test]
        public void Serializes_Pdu()
        {
            var expectedResult = new byte[] {
                0x00, 0x00, 0x00, 0x22,       // length
                0x00, 0x00, 0x00, 0x09,       // command
                0x00, 0x00, 0x00, 0x00,       // status
                0x00, 0x00, 0x00, 0x10,       // sequence
                0x54, 0x45, 0x53, 0x54, 0x00, // system ID
                0x50, 0x41, 0x53, 0x53, 0x00, // password
                0x53, 0x4d, 0x53, 0x00,       // system type
                0x34,                         // interface version
                0x02,                         // address ton
                0x03,                         // address npi
                0x00,                         // address range
            };
            var pdu = new BindTransceiver
            {
                SequenceNumber = 16,
                SystemId = "TEST",
                Password = "PASS",
                SystemType = "SMS",
                InterfaceVersion = 52,
                AddressTon = TypeOfNumber.National,
                AddressNpi = NumericPlanIndicator.Telex
            };

            var serializer = new BindTransceiverSerializer();
            var result = serializer.Serialize(pdu);

            CollectionAssert.AreEqual(expectedResult, result);
        }
    }
}
