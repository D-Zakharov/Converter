namespace Converter.Domain.Entities.Exceptions
{
    public class FileIsMissingException : CriticalConverterException
    {
        public override string Message { get; }

        public FileIsMissingException(int fileId)
        {
            Message = $"Input file with id {fileId} is missing";
        }
    }
}
