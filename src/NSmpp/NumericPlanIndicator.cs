namespace NSmpp
{
    /// <summary>
    /// These fields define the Numeric Plan Indicator (NPI) 
    /// to be used in the SME address parameters.
    /// </summary>
    public enum NumericPlanIndicator : byte
    {
        Unknown,
        Isdn,
        Data,
        Telex,
        LandMobile,
        National,
        Private,
        Ermes,
        Internet,
        Wap
    }
}
