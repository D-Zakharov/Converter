using Converter.Domain.Services;
using Converter.WebService.Models;
using Converter.WebService.Services;

namespace Converter.WebService.Endpoints
{
    public static class ServiceEndpoints
    {
        private const string OutputContentType = "application/pdf";
        private static string ResultFileName => $"{DateTime.Now:yyyy-MM-dd_HH-mm}.pdf";

        public static void MapEndpoints(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.MapPost("/api/v1/convert/HtmlToPdf", SendFileToConversion).Accepts<ConvertRequestModel>("multipart/form-data"); ;
                app.MapGet("/api/v1/convert/GetResult", GetResult);
                app.MapGet("/api/v1/convert/CheckResult", CheckResult);
            }
        }

        private static async Task<IResult> SendFileToConversion(HttpContext ctx, IQueueManager queueManager)
        {
            ConvertRequestModel requestData = new(ctx);
            await queueManager.SendToConversionQueue(requestData.IdempotencyKey, requestData.Content);

            return Results.Ok();
        }

        private static IResult GetResult(Guid requestKey, IDataStorage resultFilesStorage)
        {
            var fileStream = resultFilesStorage.LoadResultFileByRequestGuid(requestKey);
            if (fileStream is null)
                return Results.BadRequest();

            return Results.File(fileStream, OutputContentType, ResultFileName);
        }

        private static IResult CheckResult(Guid requestKey, IDataStorage resultFilesStorage)
        {
            bool isReady = resultFilesStorage.IsResultReady(requestKey);

            return Results.Json(new { IsReady = isReady });
        }
    }
}
