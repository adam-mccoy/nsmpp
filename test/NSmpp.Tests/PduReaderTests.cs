using System;
using NUnit.Framework;
using NSmpp.Serialization;

namespace NSmpp.Tests
{
    [TestFixture]
    public class PduReaderTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException), ExpectedMessage = "Value cannot be null.\r\nParameter name: buffer")]
        public void Throws_On_Null_Buffer()
        {
            new PduReader(null);
        }

        [Test]
        public void Reads_Four_Byte_Integer()
        {
            var buffer = new byte[] { 0x00, 0x00, 0x30, 0x39 };

            var reader = new PduReader(buffer);

            Assert.AreEqual(12345, reader.ReadInteger());
        }

        [Test]
        public void Reads_Two_Byte_Integer()
        {
            var buffer = new byte[] { 0x74, 0x8a };

            var reader = new PduReader(buffer);

            Assert.AreEqual(29834, reader.ReadShort());
        }

        [Test]
        public void Reads_Single_Byte_Integer()
        {
            var buffer = new byte[] { 0xc4 };

            var reader = new PduReader(buffer);

            Assert.AreEqual(196, reader.ReadByte());
        }

        [Test]
        public void Reads_Null_String()
        {
            var buffer = new byte[] { 0x00 };

            var reader = new PduReader(buffer);

            Assert.IsNull(reader.ReadString());
        }

        [Test]
        public void Reads_String()
        {
            var buffer = new byte[] {
                0x48, 0x65, 0x6c, 0x6c, 0x6f, 0x00
            };

            var reader = new PduReader(buffer);

            Assert.AreEqual("Hello", reader.ReadString());
        }

        [Test]
        public void Reads_Bytes()
        {
            var expectedResult = new byte[] {
                0x34, 0x11
            };
            var buffer = new byte[] {
                0x34, 0x11, 0xcc, 0x7a, 0xef
            };

            var reader = new PduReader(buffer);

            CollectionAssert.AreEqual(expectedResult, reader.ReadBytes(2));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Buffer does not contain 4 bytes.")]
        public void Throw_On_Read_Four_Byte_Integer_With_Insufficient_Bytes()
        {
            var buffer = new byte[] {
                0x63, 0xf1, 0x23
            };

            var reader = new PduReader(buffer);

            reader.ReadInteger();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Buffer does not contain 2 bytes.")]
        public void Throw_On_Read_Two_Byte_Integer_With_Insufficient_Bytes()
        {
            var buffer = new byte[] {
                0x63
            };

            var reader = new PduReader(buffer);

            reader.ReadShort();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Buffer does not contain 1 bytes.")]
        public void Throw_On_Read_Byte_With_Insufficient_Bytes()
        {
            var buffer = new byte[0];

            var reader = new PduReader(buffer);

            reader.ReadByte();
        }
    }
}
