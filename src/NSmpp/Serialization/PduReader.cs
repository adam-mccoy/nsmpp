using System;
using System.Text;

namespace NSmpp.Serialization
{
    internal class PduReader
    {
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
            EnsureSize(4);

            int value = 0;
            value |= _buffer[_position++] << 24;
            value |= _buffer[_position++] << 16;
            value |= _buffer[_position++] << 8;
            value |= _buffer[_position++];
            return value;
        }

        internal short ReadShort()
        {
            EnsureSize(2);

            short value = 0;
            value |= (short)(_buffer[_position++] << 8);
            value |= (short)(_buffer[_position++]);
            return value;
        }

        internal byte ReadByte()
        {
            EnsureSize(1);

            return _buffer[_position++];
        }

        internal string ReadString()
        {
            var builder = new StringBuilder();
            while (_buffer[_position] != 0x00)
            {
                builder.Append((char)_buffer[_position++]);
            }
            _position++;
            return builder.Length == 0 ? null : builder.ToString();
        }

        internal byte[] ReadBytes(int count)
        {
            var bytes = new byte[count];
            Buffer.BlockCopy(_buffer, _position, bytes, 0, count);
            _position += count;
            return bytes;
        }

        private void EnsureSize(int size)
        {
            if (_buffer.Length - _position < size)
                throw new InvalidOperationException(
                    string.Format("Buffer does not contain {0} bytes.", size));
        }
    }
}
