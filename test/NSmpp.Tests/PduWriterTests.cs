using FluentAssertions;
using NSmpp.Serialization;
using NSubstitute.ExceptionExtensions;
using static FluentAssertions.FluentActions;

namespace NSmpp.Tests;

[TestFixture]
public class PduWriterTests
{
    [Test]
    public void Get_Bytes_Returns_Empty_Array()
    {
        var builder = new PduWriter();
        var bytes = builder.GetBytes();

        bytes.Length.Should().Be(0);
    }

    [Test]
    public void Writes_Four_Byte_Integer()
    {
        byte[] expectedResult = [0x49, 0x63, 0x18, 0xff];
        var builder = new PduWriter();

        builder.WriteInt32(1231231231);
        var buffer = builder.GetBytes();

        buffer.Should().Equal(expectedResult);
    }

    [Test]
    public void Writes_Two_Byte_Integer()
    {
        byte[] expectedResult = [0x26, 0x94];
        var builder = new PduWriter();

        const short testValue = 9876;
        builder.WriteInt16(testValue);
        var buffer = builder.GetBytes();

        buffer.Should().Equal(expectedResult);
    }

    [Test]
    public void Writes_Single_Byte_Integer()
    {
        byte[] expectedResult = [0x42];
        var builder = new PduWriter();

        const byte testValue = 66;
        builder.WriteByte(testValue);
        var buffer = builder.GetBytes();

        buffer.Should().Equal(expectedResult);
    }

    [Test]
    public void Writes_Null_String()
    {
        byte[] expectedResult = [0x00];
        var builder = new PduWriter();

        builder.WriteString(null);
        var buffer = builder.GetBytes();

        buffer.Should().Equal(expectedResult);
    }

    [Test]
    public void Writes_Null_Terminated_String()
    {
        byte[] expectedResult =
        [
            0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x00
        ];
        var builder = new PduWriter();

        builder.WriteString("abcdefgh");
        var buffer = builder.GetBytes();

        buffer.Should().Equal(expectedResult);
    }

    [Test]
    public void Writes_Bytes()
    {
        byte[] expectedResult =
        [
            0x42, 0x7a, 0xc1, 0x35, 0xff
        ];
        var builder = new PduWriter();

        builder.WriteBytes(expectedResult);
        var buffer = builder.GetBytes();

        buffer.Should().Equal(expectedResult);
    }

    [Test]
    public void Appends_Four_Byte_Integer()
    {
        byte[] expectedResult =
        [
            0x00, 0x00, 0xfe, 0xf7,
            0x00, 0x01, 0x36, 0x16
        ];
        var builder = new PduWriter();

        builder.WriteInt32(65271);
        builder.WriteInt32(79382);
        var buffer = builder.GetBytes();

        buffer.Should().Equal(expectedResult);
    }

    [Test]
    public void Appends_Two_Byte_Integer()
    {
        byte[] expectedResult =
        [
            0x07, 0x28,
            0x00, 0x01, 0x36, 0x16
        ];
        var builder = new PduWriter();

        const short testValue = 1832;
        builder.WriteInt16(testValue);
        builder.WriteInt32(79382);
        var buffer = builder.GetBytes();

        buffer.Should().Equal(expectedResult);
    }

    [Test]
    public void Appends_Single_Byte_Integer()
    {
        byte[] expectedResult =
        [
            0x24,
            0x00, 0x01, 0x36, 0x16
        ];
        var builder = new PduWriter();

        const byte testValue = 0x24;
        builder.WriteByte(testValue);
        builder.WriteInt32(79382);
        var buffer = builder.GetBytes();

        buffer.Should().Equal(expectedResult);
    }

    [Test]
    public void Appends_String()
    {
        byte[] expectedResult =
        [
            0x48, 0x65, 0x6c, 0x6c, 0x6f, 0x00,
            0x00, 0x01, 0x36, 0x16
        ];
        var builder = new PduWriter();

        builder.WriteString("Hello");
        builder.WriteInt32(79382);
        var buffer = builder.GetBytes();

        buffer.Should().Equal(expectedResult);
    }

    [Test]
    public void Appends_Bytes()
    {
        byte[] expectedResult =
        [
            0x34, 0x11, 0xcc, 0x7a, 0xef,
            0x00, 0x01, 0x36, 0x16
        ];
        var builder = new PduWriter();

        var testValue = new byte[] { 0x34, 0x11, 0xcc, 0x7a, 0xef };
        builder.WriteBytes(testValue);
        builder.WriteInt32(79382);
        var buffer = builder.GetBytes();

        buffer.Should().Equal(expectedResult);
    }

    [Test]
    public void Throws_On_Negative_Initial_Buffer_Size()
    {
        Invoking(() => new PduWriter(-1))
            .Should()
            .ThrowExactly<ArgumentOutOfRangeException>()
            .Which.Message.Should().Be("Value must be zero or greater (Parameter 'initialBufferSize')");
    }

    [Test]
    public void Resizes_Buffer_On_Write_Four_Byte_Integer()
    {
        byte[] expectedResult =
        [
            0x00, 0x01, 0x36, 0x16
        ];
        var builder = new PduWriter(0);
        builder.WriteInt32(79382);
        var buffer = builder.GetBytes();

        buffer.Should().Equal(expectedResult);
    }

    [Test]
    public void Resizes_Buffer_On_Write_Two_Byte_Integer()
    {
        byte[] expectedResult =
        [
            0x07, 0x28
        ];
        var builder = new PduWriter(0);
        const short testValue = 1832;
        builder.WriteInt16(testValue);
        var buffer = builder.GetBytes();

        buffer.Should().Equal(expectedResult);
    }

    [Test]
    public void Resizes_Buffer_On_Write_Single_Byte_Integer()
    {
        byte[] expectedResult = [0xc4];
        var builder = new PduWriter(0);
        const byte testValue = 0xc4;
        builder.WriteByte(testValue);
        var buffer = builder.GetBytes();

        buffer.Should().Equal(expectedResult);
    }

    [Test]
    public void Resizes_Buffer_On_Write_Bytes()
    {
        byte[] expectedResult =
        [
            0x34, 0x11, 0xcc, 0x7a, 0xef
        ];
        var builder = new PduWriter(0);
        var testValue = new byte[] { 0x34, 0x11, 0xcc, 0x7a, 0xef };
        builder.WriteBytes(testValue);
        var buffer = builder.GetBytes();

        buffer.Should().Equal(expectedResult);
    }

    [Test]
    public void Resizes_Buffer_On_Write_String()
    {
        byte[] expectedResult =
        [
            0x48, 0x65, 0x6c, 0x6c, 0x6f, 0x00
        ];
        var builder = new PduWriter(0);
        builder.WriteString("Hello");
        var buffer = builder.GetBytes();

        buffer.Should().Equal(expectedResult);
    }

    [Test]
    public void Resizes_Buffer_On_Write_Null_String()
    {
        byte[] expectedResult = [0x00];
        var builder = new PduWriter(0);
        builder.WriteString(null);
        var buffer = builder.GetBytes();

        buffer.Should().Equal(expectedResult);
    }

    [Test]
    public void Throws_On_Write_Null_Bytes()
    {
        var builder = new PduWriter();
        Invoking(() => builder.WriteBytes(null!))
            .Should()
            .ThrowExactly<ArgumentNullException>()
            .Which.Message.Should().Be("Value cannot be null. (Parameter 'bytes')");
    }

    [Test]
    public void Write_Absolute_Time_Writes_Zero_Byte_For_Null_Value()
    {
        byte[] expectedResult = [0x00];
        var builder = new PduWriter();
        builder.WriteAbsoluteTime(null);
        var buffer = builder.GetBytes();

        buffer.Should().Equal(expectedResult);
    }

    [Test]
    public void Write_Aboslute_Time_Writes_Correct_String()
    {
        byte[] expectedResult =
        [
            0x31, 0x36, 0x30, 0x38, 0x31, 0x38, 0x31, 0x36, 0x33,
            0x34, 0x35, 0x36, 0x34, 0x34, 0x30, 0x2b, 0x00
        ];

        var builder = new PduWriter();
        var time = new DateTimeOffset(2016, 8, 18, 16, 34, 56, 400, TimeSpan.FromHours(10));
        builder.WriteAbsoluteTime(time);
        var buffer = builder.GetBytes();

        buffer.Should().Equal(expectedResult);
    }
}