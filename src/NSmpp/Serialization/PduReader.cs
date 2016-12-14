using System;
using System.Globalization;
using System.Text;

namespace NSmpp.Serialization
{
    internal class PduReader
    {
        private const int AbsoluteTimeLength = 16;

        private readonly byte[] _buffer;
        private int _position = 0;

        public PduReader(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            _buffer = buffer;
        }

        internal int ReadInteger()
        {
            var value = ReadInteger(_buffer, _position);
            _position += 4;
            return value;
        }

        internal static int ReadInteger(byte[] buffer, int position)
        {
            EnsureSize(buffer, position, 4);

            int value = 0;
            value |= buffer[position++] << 24;
            value |= buffer[position++] << 16;
            value |= buffer[position++] << 8;
            value |= buffer[position++];
            return value;
        }

        internal short ReadShort()
        {
            var value = ReadShort(_buffer, _position);
            _position += 2;
            return value;
        }

        internal static short ReadShort(byte[] buffer, int position)
        {
            EnsureSize(buffer, position, 2);

            short value = 0;
            value |= (short)(buffer[position++] << 8);
            value |= (short)(buffer[position++]);
            return value;
        }

        internal byte ReadByte()
        {
            return ReadByte(_buffer, _position++);
        }

        internal static byte ReadByte(byte[] buffer, int position)
        {
            EnsureSize(buffer, position, 1);

            return buffer[position++];
        }

        internal string ReadString()
        {
            var result = ReadString(_buffer, _position);
            _position += (result?.Length ?? 0) + 1;
            return result;
        }

        internal DateTimeOffset? ReadAbsoluteTime()
        {
            var timeString = ReadString();
            if (timeString == null)
                return null;

            if (timeString.Length != AbsoluteTimeLength)
                throw new ArgumentException("Absolute time has an invalid length.");

            DateTime dateTime;
            if (!DateTime.TryParseExact(timeString.Substring(0, 13), "yyMMddHHmmssf",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                throw new ArgumentException("Absolute time has an invalid format.");

            float quarterHours;
            if (!float.TryParse(timeString.Substring(13, 2), out quarterHours))
                throw new ArgumentException("Absolute time has an invalid format.");

            var multiplier = timeString[timeString.Length - 1] == '-' ? -15.0f : 15.0f;
            return new DateTimeOffset(dateTime, TimeSpan.FromMinutes(quarterHours * multiplier));
        }

        internal static string ReadString(byte[] buffer, int position)
        {
            var builder = new StringBuilder();
            while (position < buffer.Length && buffer[position] != 0x00)
            {
                builder.Append((char)buffer[position++]);
            }
            return builder.Length == 0 ? null : builder.ToString();
        }

        internal byte[] ReadBytes(int count)
        {
            var result = ReadBytes(_buffer, _position, count);
            _position += result.Length;
            return result;
        }

        internal static byte[] ReadBytes(byte[] buffer, int position, int count)
        {
            var bytes = new byte[count];
            Buffer.BlockCopy(buffer, position, bytes, 0, count);
            return bytes;
        }

        internal Address ReadAddress()
        {
            var result = ReadAddress(_buffer, _position);
            _position += result == null ? 3 : result.Value.Length + 3;
            return result;
        }

        internal static Address ReadAddress(byte[] buffer, int position)
        {
            var ton = (TypeOfNumber)ReadByte(buffer, position++);
            var npi = (NumericPlanIndicator)ReadByte(buffer, position++);
            var value = ReadString(buffer, position);

            if (value == null)
                return null;
            return new Address(ton, npi, value);
        }

        internal void Skip(int numBytes)
        {
            EnsureSize(_buffer, _position, numBytes);
            _position += numBytes;
        }

        private static void EnsureSize(byte[] buffer, int position, int size)
        {
            if (buffer.Length - position < size)
                throw new InvalidOperationException(
                    string.Format("Buffer does not contain {0} bytes.", size));
        }
    }
}
