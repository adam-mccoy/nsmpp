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

        public static bool operator ==(Address a1, Address a2)
        {
            if (ReferenceEquals(a1, a2)) return true;
            if ((object)a1 == null || (object)a2 == null) return false;
            return a1.Equals(a2);
        }

        public static bool operator !=(Address a1, Address a2)
        {
            return !(a1 == a2);
        }

        public override bool Equals(object obj)
        {
            var rhs = obj as Address;
            if ((object)rhs == null)
                return false;

            return rhs.Ton == Ton &&
                   rhs.Npi == Npi &&
                   rhs.Value == Value;
        }

        public override int GetHashCode()
        {
            var hash = 13;
            hash = hash * 7 + Ton.GetHashCode();
            hash = hash * 7 + Npi.GetHashCode();
            hash = hash * 7 + Value.GetHashCode();

            return hash;
        }

        public override string ToString()
        {
            return $"{Ton}:{Npi}:{Value}";
        }
    }
}
