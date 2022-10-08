namespace Converter.Domain.Entities.Exceptions
{
    public class InputFileIsMissingException : CriticalConverterException
    {
        public override string Message { get; }

        public InputFileIsMissingException(Guid requestGuid)
        {
            Message = $"Input file is missing in request with guid {requestGuid} is missing";
        }
    }
}
