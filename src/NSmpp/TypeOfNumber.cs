namespace NSmpp
{
    /// <summary>
    /// These fields define the Type of Number (TON) to be 
    /// used in the SME address parameters.
    /// </summary>
    public enum TypeOfNumber : byte
    {
        Unknown,
        International,
        National,
        NetworkSpecific,
        SubscriberNumber,
        Alphanumeric,
        Abbreviated
    }
}
