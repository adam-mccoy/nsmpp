﻿using NSmpp.Serialization;
using NUnit.Framework;
using System;

namespace NSmpp.Tests
{
    [TestFixture]
    public class PduWriterTests
    {
        [Test]
        public void Get_Bytes_Returns_Empty_Array()
        {
            var builder = new PduWriter();
            var bytes = builder.GetBytes();

            Assert.AreEqual(0, bytes.Length);
        }

        [Test]
        public void Writes_Four_Byte_Integer()
        {
            var expectedResult = new byte[] {
                0x49, 0x63, 0x18, 0xff
            };
            var builder = new PduWriter();

            builder.WriteInteger(1231231231);
            var buffer = builder.GetBytes();

            CollectionAssert.AreEqual(expectedResult, buffer);
        }

        [Test]
        public void Writes_Two_Byte_Integer()
        {
            var expectedResult = new byte[] {
                0x26, 0x94
            };
            var builder = new PduWriter();

            short testValue = 9876;
            builder.WriteShort(testValue);
            var buffer = builder.GetBytes();

            CollectionAssert.AreEqual(expectedResult, buffer);
        }

        [Test]
        public void Writes_Single_Byte_Integer()
        {
            var expectedResult = new byte[] {
                0x42
            };
            var builder = new PduWriter();

            byte testValue = 66;
            builder.WriteByte(testValue);
            var buffer = builder.GetBytes();

            CollectionAssert.AreEqual(expectedResult, buffer);
        }

        [Test]
        public void Writes_Null_String()
        {
            var expectedResult = new byte[] { 0x00 };
            var builder = new PduWriter();

            builder.WriteString(null);
            var buffer = builder.GetBytes();

            CollectionAssert.AreEqual(expectedResult, buffer);
        }

        [Test]
        public void Writes_Null_Terminated_String()
        {
            var expectedResult = new byte[] {
                0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x00
            };
            var builder = new PduWriter();

            builder.WriteString("abcdefgh");
            var buffer = builder.GetBytes();

            CollectionAssert.AreEqual(expectedResult, buffer);
        }

        [Test]
        public void Writes_Bytes()
        {
            var expectedResult = new byte[] {
                0x42, 0x7a, 0xc1, 0x35, 0xff
            };
            var builder = new PduWriter();

            builder.WriteBytes(expectedResult);
            var buffer = builder.GetBytes();

            CollectionAssert.AreEqual(expectedResult, buffer);
        }

        [Test]
        public void Appends_Four_Byte_Integer()
        {
            var expectedResult = new byte[] {
                0x00, 0x00, 0xfe, 0xf7,
                0x00, 0x01, 0x36, 0x16
            };
            var builder = new PduWriter();

            builder.WriteInteger(65271);
            builder.WriteInteger(79382);
            var buffer = builder.GetBytes();

            CollectionAssert.AreEqual(expectedResult, buffer);
        }

        [Test]
        public void Appends_Two_Byte_Integer()
        {
            var expectedResult = new byte[] {
                0x07, 0x28,
                0x00, 0x01, 0x36, 0x16
            };
            var builder = new PduWriter();

            short testValue = 1832;
            builder.WriteShort(testValue);
            builder.WriteInteger(79382);
            var buffer = builder.GetBytes();

            CollectionAssert.AreEqual(expectedResult, buffer);
        }

        [Test]
        public void Appends_Single_Byte_Integer()
        {
            var expectedResult = new byte[] {
                0x24,
                0x00, 0x01, 0x36, 0x16
            };
            var builder = new PduWriter();

            byte testValue = 0x24;
            builder.WriteByte(testValue);
            builder.WriteInteger(79382);
            var buffer = builder.GetBytes();

            CollectionAssert.AreEqual(expectedResult, buffer);
        }

        [Test]
        public void Appends_String()
        {
            var expectedResult = new byte[] {
                0x48, 0x65, 0x6c, 0x6c, 0x6f, 0x00,
                0x00, 0x01, 0x36, 0x16
            };
            var builder = new PduWriter();

            builder.WriteString("Hello");
            builder.WriteInteger(79382);
            var buffer = builder.GetBytes();

            CollectionAssert.AreEqual(expectedResult, buffer);
        }

        [Test]
        public void Appends_Bytes()
        {
            var expectedResult = new byte[] {
                0x34, 0x11, 0xcc, 0x7a, 0xef,
                0x00, 0x01, 0x36, 0x16
            };
            var builder = new PduWriter();

            var testValue = new byte[] { 0x34, 0x11, 0xcc, 0x7a, 0xef };
            builder.WriteBytes(testValue);
            builder.WriteInteger(79382);
            var buffer = builder.GetBytes();

            CollectionAssert.AreEqual(expectedResult, buffer);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException), ExpectedMessage = "Value must be zero or greater.\r\nParameter name: initialBufferSize")]
        public void Throws_On_Negative_Initial_Buffer_Size()
        {
            var builder = new PduWriter(-1);
        }

        [Test]
        public void Resizes_Buffer_On_Write_Four_Byte_Integer()
        {
            var expectedResult = new byte[] {
                0x00, 0x01, 0x36, 0x16
            };
            var builder = new PduWriter(0);
            builder.WriteInteger(79382);
            var buffer = builder.GetBytes();

            CollectionAssert.AreEqual(expectedResult, buffer);
        }

        [Test]
        public void Resizes_Buffer_On_Write_Two_Byte_Integer()
        {
            var expectedResult = new byte[] {
                0x07, 0x28
            };
            var builder = new PduWriter(0);
            short testValue = 1832;
            builder.WriteShort(testValue);
            var buffer = builder.GetBytes();

            CollectionAssert.AreEqual(expectedResult, buffer);
        }

        [Test]
        public void Resizes_Buffer_On_Write_Single_Byte_Integer()
        {
            var expectedResult = new byte[] {
                0xc4
            };
            var builder = new PduWriter(0);
            byte testValue = 0xc4;
            builder.WriteByte(testValue);
            var buffer = builder.GetBytes();

            CollectionAssert.AreEqual(expectedResult, buffer);
        }

        [Test]
        public void Resizes_Buffer_On_Write_Bytes()
        {
            var expectedResult = new byte[] {
                0x34, 0x11, 0xcc, 0x7a, 0xef,
            };
            var builder = new PduWriter(0);
            var testValue = new byte[] { 0x34, 0x11, 0xcc, 0x7a, 0xef };
            builder.WriteBytes(testValue);
            var buffer = builder.GetBytes();

            CollectionAssert.AreEqual(expectedResult, buffer);
        }

        [Test]
        public void Resizes_Buffer_On_Write_String()
        {
            var expectedResult = new byte[] {
                0x48, 0x65, 0x6c, 0x6c, 0x6f, 0x00,
            };
            var builder = new PduWriter(0);
            builder.WriteString("Hello");
            var buffer = builder.GetBytes();

            CollectionAssert.AreEqual(expectedResult, buffer);
        }

        [Test]
        public void Resizes_Buffer_On_Write_Null_String()
        {
            var expectedResult = new byte[] {
                0x00,
            };
            var builder = new PduWriter(0);
            builder.WriteString(null);
            var buffer = builder.GetBytes();

            CollectionAssert.AreEqual(expectedResult, buffer);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException), ExpectedMessage = "Value cannot be null.\r\nParameter name: bytes")]
        public void Throws_On_Write_Null_Bytes()
        {
            var builder = new PduWriter();
            builder.WriteBytes(null);
        }
    }
}
