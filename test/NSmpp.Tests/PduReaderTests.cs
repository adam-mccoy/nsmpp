using FluentAssertions;
using NSmpp.Serialization;
using static FluentAssertions.FluentActions;

namespace NSmpp.Tests;

[TestFixture]
public class PduReaderTests
{
    [Test]
    public void Throws_On_Null_Buffer()
    {
        Invoking(() => new PduReader(null!))
            .Should()
            .ThrowExactly<ArgumentNullException>()
            .Which.Message.Should().Be("Value cannot be null. (Parameter 'buffer')");
    }

    [Test]
    public void Reads_Four_Byte_Integer()
    {
        byte[] buffer = [0x00, 0x00, 0x30, 0x39];

        var reader = new PduReader(buffer);
        var result = reader.ReadInt32();

        result.Should().Be(12345);
    }

    [Test]
    public void Reads_Two_Byte_Integer()
    {
        byte[] buffer = [0x74, 0x8a];

        var reader = new PduReader(buffer);
        var result = reader.ReadInt16();

        result.Should().Be(29834);
    }

    [Test]
    public void Reads_Single_Byte_Integer()
    {
        byte[] buffer = [0xc4];

        var reader = new PduReader(buffer);
        var result = reader.ReadByte();

        result.Should().Be(196);
    }

    [Test]
    public void Reads_Null_String()
    {
        byte[] buffer = [0x00];

        var reader = new PduReader(buffer);
        var result = reader.ReadString();

        result.Should().BeNull();
    }

    [Test]
    public void Reads_String()
    {
        byte[] buffer = [0x48, 0x65, 0x6c, 0x6c, 0x6f, 0x00];

        var reader = new PduReader(buffer);
        var result = reader.ReadString();

        result.Should().Be("Hello");
    }

    [Test]
    public void Reads_Bytes()
    {
        byte[] expectedResult = [0x34, 0x11];
        byte[] buffer = [0x34, 0x11, 0xcc, 0x7a, 0xef];

        var reader = new PduReader(buffer);
        var result = reader.ReadBytes(2);

        result.Should().Equal(expectedResult);
    }

    [Test]
    public void Throw_On_Read_Four_Byte_Integer_With_Insufficient_Bytes()
    {
        byte[] buffer = [0x63, 0xf1, 0x23];

        var reader = new PduReader(buffer);

        Invoking(() => reader.ReadInt32())
            .Should()
            .ThrowExactly<InvalidOperationException>()
            .Which.Message.Should().Be("Buffer does not contain 4 bytes");
    }

    [Test]
    public void Throw_On_Read_Two_Byte_Integer_With_Insufficient_Bytes()
    {
        byte[] buffer = [0x63];

        var reader = new PduReader(buffer);

        Invoking(() => reader.ReadInt16())
            .Should()
            .ThrowExactly<InvalidOperationException>()
            .Which.Message.Should().Be("Buffer does not contain 2 bytes");
    }

    [Test]
    public void Throw_On_Read_Byte_With_Insufficient_Bytes()
    {
        var buffer = Array.Empty<byte>();

        var reader = new PduReader(buffer);

        Invoking(() => reader.ReadByte())
            .Should()
            .ThrowExactly<InvalidOperationException>()
            .Which.Message.Should().Be("Buffer does not contain 1 bytes");
    }

    [Test]
    public void Read_Absolute_Time_Returns_Null_For_Zero_Byte()
    {
        byte[] buffer = [0x00, 0x12, 0xc4];

        var reader = new PduReader(buffer);
        var result = reader.ReadAbsoluteTime();

        result.Should().BeNull();
    }

    [Test]
    public void Read_Absolute_Time_Returns_Correct_DateTimeOffset()
    {
        byte[] buffer =
        [
            0x31, 0x36, 0x31, 0x32, 0x31, 0x30, 0x31, 0x34, 0x35,
            0x33, 0x32, 0x34, 0x37, 0x34, 0x30, 0x2b, 0x00
        ];

        var reader = new PduReader(buffer);
        var result = reader.ReadAbsoluteTime();

        result.Should().Be(new DateTimeOffset(2016, 12, 10, 14, 53, 24, 700, TimeSpan.FromHours(10)));
    }

    [Test]
    public void Throw_On_Read_Absolute_Time_When_String_Has_Invalid_Length()
    {
        byte[] buffer =
        [
            0x31, 0x36, 0x31, 0x33, 0x31, 0x30, 0x31, 0x34, 0x35,
            0x33, 0x32, 0x34, 0x37, 0x00
        ];

        var reader = new PduReader(buffer);

        Invoking(() => reader.ReadAbsoluteTime())
            .Should()
            .ThrowExactly<ArgumentException>()
            .Which.Message.Should().Be("Absolute time has an invalid length");
    }

    [Test]
    public void Throw_On_Read_Absolute_Time_With_Invalid_Format()
    {
        byte[] buffer =
        [
            0x31, 0x36, 0x31, 0x33, 0x31, 0x30, 0x31, 0x34, 0x35,
            0x33, 0x32, 0x34, 0x37, 0x34, 0x30, 0x2b, 0x00
        ];

        var reader = new PduReader(buffer);

        Invoking(() => reader.ReadAbsoluteTime())
            .Should()
            .ThrowExactly<ArgumentException>()
            .Which.Message.Should().Be("Absolute time has an invalid format");
    }
}