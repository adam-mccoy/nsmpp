namespace NSmpp.Pdu;

internal record ParameterDefinition(int Tag, SmppType Type, int MinLength, int MaxLength);

internal enum SmppType
{
    Number,
    Binary,
    String
}