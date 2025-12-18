namespace NSmpp.Pdu;

internal static class ParameterDefinitions
{
    public static ParameterDefinition SystemId = new(-1, SmppType.String, 1, 16);
    public static ParameterDefinition Password = new(-1, SmppType.String, 0, 9);
    public static ParameterDefinition SystemType = new(-1, SmppType.String, 0, 13);
    public static ParameterDefinition InterfaceVersion = new(-1, SmppType.Number, 1, 1);
    public static ParameterDefinition AddrTon = new(-1, SmppType.Number, 1, 1);
    public static ParameterDefinition AddrNpi = new(-1, SmppType.Number, 1, 1);
    public static ParameterDefinition AddressRange = new(-1, SmppType.String, 0, 41);
}