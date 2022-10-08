using Converter.Domain.DB;
using Converter.Domain.DB.Models;
using Converter.Domain.Entities.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Converter.Domain.Services
{
    public class RequestData : IRequestData
    {
        private readonly ConverterDbContext _dbCtx;

        public RequestData(ConverterDbContext dbCtx)
        {
            _dbCtx = dbCtx;
        }

        public ConvertRequest GetInfo(Guid requestKey)
        {
            var res = _dbCtx.ConvertRequests.Find(requestKey);
            if (res is null)
                throw new RequestIsMissingException(requestKey);

            return res;
        }

        public void RemoveInfo(ConvertRequest requestData)
        {
            _dbCtx.ConvertRequests.Remove(requestData);
            _dbCtx.SaveChanges();
        }

        public ConvertRequest? SaveInfo(Guid requestKey, int? inputFileId = null, int? outputFileId = null)
        {
            try
            {
                var fileData = new ConvertRequest()
                {
                    RequestKey = requestKey,
                    InputFileId = inputFileId,
                    OutputFileId = outputFileId
                };
                _dbCtx.ConvertRequests.Add(fileData);

                _dbCtx.SaveChanges();

                return fileData;
            }
            catch (DbUpdateException)
            {
                return null;
            }
        }

        public void UpdateInfo(ConvertRequest request, int? inputFileId, int? outputFileId)
        {
            request.InputFileId = inputFileId;
            request.OutputFileId = outputFileId;
            _dbCtx.SaveChanges();
        }
    }
}
