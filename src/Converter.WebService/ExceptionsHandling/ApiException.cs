using System.Text.Json;

namespace Converter.WebService.ExceptionsHandling;

public class ApiException : Exception
{
    public int HttpCode { get; init; }
    public ErrorCodes ErrorCode { get; init; }
    public string? Error { get; init; }

    public ApiException(ErrorCodes errorCode, int httpCode = 500, string? error = null)
    {
        (HttpCode, ErrorCode, Error) = (httpCode, errorCode, error);

        if (error is null)
            Error = GetErrorMessage(errorCode);
    }

    public string GetJsonResponce()
    {
        return JsonSerializer.Serialize(this);
    }

    private static string? GetErrorMessage(ErrorCodes errorCode)
    {
        if (DefaultExceptionMessages.Messages.ContainsKey(errorCode))
            return DefaultExceptionMessages.Messages[errorCode];
        else
            return errorCode.ToString();
    }
}

public enum ErrorCodes
{
    Unknown,
    ArgumentIsNull
}
