using System.ComponentModel.DataAnnotations;
using Converter.WebService.ExceptionsHandling;

namespace Converter.WebService.Models
{
    public class ConvertRequestModel
    {
        [Required]
        public Guid IdempotencyKey { get; set; }

        [Required]
        public IFormFile Content { get; set; } = default!;

        /// <summary>
        /// Пока что парсится вручную, т.к. minimal apis не поддерживают файлы в formdata. Поддержку обещают добавить
        /// </summary>
        /// <param name="ctx"></param>
        public ConvertRequestModel(HttpContext ctx)
        {
            if (ctx.Request.Form.Files.Count == 0)
                throw new ApiException(ErrorCodes.ArgumentIsNull, 500, $"File is missing");
            if (string.IsNullOrEmpty(ctx.Request.Form[nameof(IdempotencyKey)]))
                throw new ApiException(ErrorCodes.ArgumentIsNull, 500, $"{nameof(IdempotencyKey)} is null");

            Content = ctx.Request.Form.Files[0];
            IdempotencyKey = new Guid(ctx.Request.Form[nameof(IdempotencyKey)]);
        }
    }
}
