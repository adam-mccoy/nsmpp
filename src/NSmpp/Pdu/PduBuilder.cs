namespace NSmpp.Pdu;

internal class PduBuilder
{
    private readonly SmppCommand _command;
    private readonly Dictionary<ParameterDefinition, object?> _parameters = new();

    private SmppStatus _status;
    private uint _sequenceNumber;

    public PduBuilder(SmppCommand command)
    {
        _command = command;
    }

    public PduBuilder WithStatus(SmppStatus status)
    {
        _status = status;
        return this;
    }

    public PduBuilder WithSequence(uint sequenceNumber)
    {
        _sequenceNumber = sequenceNumber;
        return this;
    }

    public PduBuilder WithParameter(ParameterDefinition definition, object? value)
    {
        _parameters[definition] = value;
        return this;
    }

    public ReadOnlySpan<byte> Build()
    {
        ValidateParameters();
    }

    private void ValidateParameters()
    {
        var pduDefinition = PduDefinition.Definitions[_command];
        var missingParameters = pduDefinition
            .MandatoryParameters
            .Except(_parameters.Keys)
            .ToList();

        if (missingParameters.Any())
        {
            throw new Exception($"Missing mandatory parameters for PDU {_command}");
        }

        var invalidOptionalParameters = pduDefinition
            .MandatoryParameters.Union(pduDefinition.OptionalParameters)
            .Except(_parameters.Keys)
            .ToList();

        if (invalidOptionalParameters.Any())
        {
            throw new Exception($"Found invalid optional parameters for PDU {_command}");
        }
    }
}