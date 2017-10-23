using System;
using System.Text;
using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal class PduWriter
    {
        const int DefaultBufferSize = 200;

        private static byte[] EmptyAddress = new byte[] { 0x00, 0x00, 0x00 };

        private byte[] _buffer;
        private int _position = 0;

        public PduWriter()
            : this(DefaultBufferSize)
        {
        }

        public PduWriter(int initialBufferSize)
        {
            if (initialBufferSize < 0)
                throw new ArgumentOutOfRangeException("initialBufferSize", "Value must be zero or greater.");
            _buffer = new byte[initialBufferSize];
        }

        internal byte[] GetBytes()
        {
            var result = new byte[_position];
            Buffer.BlockCopy(_buffer, 0, result, 0, _position);
            return result;
        }

        internal void WriteInteger(int value)
        {
            EnsureSize(4);
            WriteInteger(_buffer, _position, value);
            _position += 4;
        }

        internal static void WriteInteger(byte[] buffer, int position, int value)
        {
            buffer[position++] = (byte)(value >> 24 & 0xff);
            buffer[position++] = (byte)(value >> 16 & 0xff);
            buffer[position++] = (byte)(value >> 8 & 0xff);
            buffer[position++] = (byte)(value & 0xff);
        }

        internal void WriteShort(short value)
        {
            EnsureSize(2);
            _buffer[_position++] = (byte)(value >> 8 & 0xff);
            _buffer[_position++] = (byte)(value & 0xff);
        }

        internal void WriteByte(int value)
        {
            EnsureSize(1);
            _buffer[_position++] = (byte)value;
        }

        internal void WriteString(string value)
        {
            EnsureSize((value ?? string.Empty).Length + 1);
            if (!string.IsNullOrEmpty(value))
            {
                var encoded = System.Text.Encoding.ASCII.GetBytes(value, 0, value.Length, _buffer, _position);
                _position += encoded;
            }
            _buffer[_position++] = 0x00;
        }

        internal void WriteBytes(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes");
            EnsureSize(bytes.Length);
            Buffer.BlockCopy(bytes, 0, _buffer, _position, bytes.Length);
            _position += bytes.Length;
        }

        internal void WritePduHeader(PduBase pdu)
        {
            if (pdu == null)
                throw new ArgumentNullException("pdu");

            WriteInteger(0);
            WriteInteger((int)pdu.Command);

            var responsePdu = pdu as ResponsePduBase;
            if (responsePdu != null)
                WriteInteger((int)responsePdu.Status);
            else
                WriteInteger(0);
            WriteInteger((int)pdu.SequenceNumber);
        }

        internal void WriteAbsoluteTime(DateTimeOffset? time)
        {
            if (!time.HasValue)
            {
                WriteString(null);
                return;
            }

            var timeValue = time.Value;
            var timeString = timeValue.ToString("yyMMddHHmmssf");

            var offsetString = (timeValue.Offset.TotalMinutes / 15).ToString("00");
            offsetString += timeValue.Offset.Ticks < 0 ? "-" : "+";

            WriteString(timeString + offsetString);
        }

        internal void WriteAddress(Address address)
        {
            if (address == null)
            {
                WriteBytes(EmptyAddress);
            }
            else
            {
                WriteByte((byte)address.Ton);
                WriteByte((byte)address.Npi);
                WriteString(address.Value);
            }
        }

        private void EnsureSize(int size)
        {
            if (_buffer.Length - _position > size)
                return;

            var newBuffer = new byte[_buffer.Length + DefaultBufferSize];
            Buffer.BlockCopy(_buffer, 0, newBuffer, 0, _position);
            _buffer = newBuffer;
        }
    }
}
