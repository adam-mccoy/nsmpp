namespace NSmpp.Pdu;

internal class PduDefinition
{
    public ParameterDefinition[] MandatoryParameters { get; set; }
    public ParameterDefinition[] OptionalParameters { get; set; }

    public static IDictionary<SmppCommand, PduDefinition> Definitions = new Dictionary<SmppCommand, PduDefinition>()
    {
        { SmppCommand.BindReceiver, PduDefinitions.BindReceiver }
    };
}

internal static class PduDefinitions
{
    public static readonly PduDefinition BindReceiver = new()
    {
        MandatoryParameters =
        [
            ParameterDefinitions.SystemId,
            ParameterDefinitions.Password,
            ParameterDefinitions.SystemType,
            ParameterDefinitions.InterfaceVersion,
            ParameterDefinitions.AddrTon,
            ParameterDefinitions.AddrNpi,
            ParameterDefinitions.AddressRange,
        ],
        OptionalParameters = []
    };
}