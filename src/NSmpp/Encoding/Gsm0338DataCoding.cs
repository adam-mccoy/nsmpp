using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSmpp.Encoding
{
    internal class Gsm0338DataCoding : DataCoding
    {
        private const byte EscapeByte = 0x1b;
        private const char UnknownChar = '?';
        private const byte UnknownByte = 0x3f;
        private static readonly char[] CharSet = new[]
        {
            '@', '£', '$', '¥', 'è', 'é', 'ù', 'ì', 'ò', 'Ç', '\n', 'Ø', 'ø', '\r', 'Å', 'å',
            'Δ', '_', 'Φ', 'Γ', 'Λ', 'Ω', 'Π', 'Ψ', 'Σ', 'Θ', 'Ξ', '\x001b', 'Æ', 'æ', 'ß', 'É',
            ' ', '!', '"', '#', '¤', '%', '&', '\'', '(', ')', '*', '+', ',', '-', '.', '/',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ':', ';', '<', '=', '>', '?',
            '¡', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O',
            'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'Ä', 'Ö', 'Ñ', 'Ü', '§',
            '¿', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o',
            'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'ä', 'ö', 'ñ', 'ü', 'à'
        };
        private static readonly char[] ExtendedCharSet = new[]
        {
            '€', 'e',
            '\f', '\n',
            '[', '<',
            '\\', '/',
            ']', '>',
            '^', 'Λ',
            '{', '(',
            '|', '@',
            '}', ')',
            '~', '='
        };

        private static readonly Dictionary<byte, char> DecodingDict = new Dictionary<byte, char>();
        private static readonly Dictionary<char, byte> EncodingDict = new Dictionary<char, byte>();
        private static readonly Dictionary<char, char> ExtDecodingDict = new Dictionary<char, char>();
        private static readonly Dictionary<char, char> ExtEncodingDict = new Dictionary<char, char>();

        static Gsm0338DataCoding()
        {
            foreach (var e in CharSet.Select((c, i) => new { c, b = (byte)i }))
            {
                DecodingDict.Add(e.b, e.c);
                EncodingDict.Add(e.c, e.b);
            }
            for (var i = 0; i < ExtendedCharSet.Length / 2; i += 2)
            {
                ExtDecodingDict[ExtendedCharSet[i + 1]] = ExtendedCharSet[i];
                ExtEncodingDict[ExtendedCharSet[i]] = ExtendedCharSet[i + 1];
            }
        }

        public override string Decode(byte[] value)
        {
            var builder = new StringBuilder(value.Length);
            var isEscaped = false;
            foreach (var b in value)
            {
                if (b == 0x1b && !isEscaped)
                {
                    isEscaped = true;
                }
                else if (DecodingDict.TryGetValue(b, out char c))
                {
                    if (isEscaped && ExtDecodingDict.TryGetValue(c, out char x))
                        builder.Append(x);
                    else if (isEscaped)
                        builder.Append(UnknownChar);
                    else
                        builder.Append(c);
                }
                else
                {
                    builder.Append(UnknownChar);
                }
            }
            return builder.ToString();
        }

        public override byte[] Encode(string value)
        {
            var buffer = new byte[value.Length * 2];
            int pos = 0;
            for (var i = 0; i < value.Length; i++, pos++)
            {
                if (EncodingDict.TryGetValue(value[i], out byte b))
                {
                    buffer[pos] = b;
                }
                else if (ExtEncodingDict.TryGetValue(value[i], out char c))
                {
                    buffer[pos++] = EscapeByte;
                    buffer[pos] = EncodingDict[c];
                }
                else
                {
                    buffer[pos] = UnknownByte;
                }
            }

            var result = new byte[pos];
            Buffer.BlockCopy(buffer, 0, result, 0, pos);
            return result;
        }
    }
}
