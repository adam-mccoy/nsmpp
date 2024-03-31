namespace NSmpp;

public record Address(TypeOfNumber Ton, NumericPlanIndicator Npi, string Value)
{
    public Address(string value)
        : this(TypeOfNumber.Unknown, NumericPlanIndicator.Unknown, value)
    {
    }

    public static implicit operator Address(string value)
    {
        return new Address(value);
    }

    public override string ToString() => $"{Ton}:{Npi}:{Value}";
}