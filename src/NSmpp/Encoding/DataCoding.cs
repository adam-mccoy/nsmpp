namespace NSmpp.Encoding
{
    public abstract class DataCoding
    {
        public static DataCoding Default = new Gsm0338DataCoding();

        public abstract byte[] Encode(string value);
        public abstract string Decode(byte[] value);
    }
}
