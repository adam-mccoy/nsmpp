using System;
using NUnit.Framework;
using NSmpp.Serialization;

namespace NSmpp.Tests
{
    [TestFixture]
    public class PduReaderTests
    {
        [Test]
        public void Throws_On_Null_Buffer()
        {
            Assert.Throws(typeof(ArgumentNullException), () => new PduReader(null), "Value cannot be null.\r\nParameter name: buffer");
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
        public void Throw_On_Read_Four_Byte_Integer_With_Insufficient_Bytes()
        {
            var buffer = new byte[] {
                0x63, 0xf1, 0x23
            };

            var reader = new PduReader(buffer);

            Assert.Throws(typeof(InvalidOperationException), () => reader.ReadInteger(), "Buffer does not contain 4 bytes.");
        }

        [Test]
        public void Throw_On_Read_Two_Byte_Integer_With_Insufficient_Bytes()
        {
            var buffer = new byte[] {
                0x63
            };

            var reader = new PduReader(buffer);

            Assert.Throws(typeof(InvalidOperationException), () => reader.ReadShort(), "Buffer does not contain 2 bytes.");
        }

        [Test]
        public void Throw_On_Read_Byte_With_Insufficient_Bytes()
        {
            var buffer = new byte[0];

            var reader = new PduReader(buffer);

            Assert.Throws(typeof(InvalidOperationException), () => reader.ReadByte(), "Buffer does not contain 1 bytes.");
        }

        [Test]
        public void Read_Absolute_Time_Returns_Null_For_Zero_Byte()
        {
            var buffer = new byte[] { 0x00, 0x12, 0xc4 };

            var reader = new PduReader(buffer);

            Assert.IsNull(reader.ReadAbsoluteTime());
        }

        [Test]
        public void Read_Absolute_Time_Returns_Correct_DateTimeOffset()
        {
            var buffer = new byte[]
            {
                0x31, 0x36, 0x31, 0x32, 0x31, 0x30, 0x31, 0x34, 0x35,
                0x33, 0x32, 0x34, 0x37, 0x34, 0x30, 0x2b, 0x00
            };

            var reader = new PduReader(buffer);
            var result = reader.ReadAbsoluteTime();

            Assert.AreEqual(new DateTimeOffset(2016, 12, 10, 14, 53, 24, 700, TimeSpan.FromHours(10)), result);
        }

        [Test]
        public void Throw_On_Read_Absolute_Time_When_String_Has_Invalid_Length()
        {
            var buffer = new byte[]
            {
                0x31, 0x36, 0x31, 0x33, 0x31, 0x30, 0x31, 0x34, 0x35,
                0x33, 0x32, 0x34, 0x37, 0x00
            };

            var reader = new PduReader(buffer);

            var testDelegate = new TestDelegate(() =>
            {
                reader.ReadAbsoluteTime();
            });
            var ex = Assert.Throws(typeof(ArgumentException), testDelegate);
            Assert.AreEqual(ex.Message, "Absolute time has an invalid length.");
        }

        [Test]
        public void Throw_On_Read_Absolute_Time_With_Invalid_Format()
        {
            var buffer = new byte[]
            {
                0x31, 0x36, 0x31, 0x33, 0x31, 0x30, 0x31, 0x34, 0x35,
                0x33, 0x32, 0x34, 0x37, 0x34, 0x30, 0x2b, 0x00
            };

            var reader = new PduReader(buffer);

            var testDelegate = new TestDelegate(() =>
            {
                reader.ReadAbsoluteTime();
            });
            var ex = Assert.Throws(typeof(ArgumentException), testDelegate);
            Assert.AreEqual(ex.Message, "Absolute time has an invalid format.");
        }
    }
}
