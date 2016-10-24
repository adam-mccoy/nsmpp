namespace NSmpp
{
    public class Address
    {
        public Address(string value)
            : this(TypeOfNumber.Unknown, NumericPlanIndicator.Unknown, value)
        {
        }

        public Address(
            TypeOfNumber ton,
            NumericPlanIndicator npi,
            string value)
        {
            Ton = ton;
            Npi = npi;
            Value = value;
        }

        public TypeOfNumber Ton { get; }
        public NumericPlanIndicator Npi { get; }
        public string Value { get; }

        public static implicit operator Address(string value)
        {
            return new Address(value);
        }
    }
}
