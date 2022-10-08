using Converter.Domain.DB;
using Converter.Domain.DB.Models;

namespace Converter.Domain.Services
{
    public abstract class AbstractDataStorage : IDataStorage
    {
        private readonly ConverterDbContext _dbCtx;

        public AbstractDataStorage(ConverterDbContext dbCtx)
        {
            _dbCtx = dbCtx;
        }

        public void DeleteFile(int key)
        {
            DeleteFile(GetFileData(key));
        }

        public void DeleteFile(WorkerStorageFile fileData)
        {
            DeleteFileContent(fileData.StorageKey);

            _dbCtx.WorkerStorageFiles.Remove(fileData);
            _dbCtx.SaveChanges();
        }

        public Stream LoadFile(int key)
        {
            WorkerStorageFile fileData = GetFileData(key);

            return LoadFileContent(fileData.StorageKey);
        }

        public async Task<int> SaveFile(FileTypes fileType, Stream fileStream, string? fileExtension = null)
        {
            var fileKey = await SaveFileContent(fileStream, fileExtension);

            var newItem = new WorkerStorageFile() { FileType = fileType, StorageKey = fileKey, StorageType = GetStorageType() };
            _dbCtx.WorkerStorageFiles.Add(newItem);
            _dbCtx.SaveChanges();
            
            return newItem.Id;
        }

        public async Task<int> SaveFile(FileTypes fileType, string sourceFilePath, string? fileExtension)
        {
            var fileKey = await SaveFileContent(sourceFilePath, fileExtension);

            var newItem = new WorkerStorageFile() { FileType = fileType, StorageKey = fileKey, StorageType = GetStorageType() };
            _dbCtx.WorkerStorageFiles.Add(newItem);
            _dbCtx.SaveChanges();

            return newItem.Id;
        }

        public Stream? LoadResultFileByRequestGuid(Guid requestKey)
        {
            var requestData = GetRequestData(requestKey);

            if (requestData.OutputFileId is null)
                return null;

            return LoadFile(requestData.OutputFileId.Value);
        }

        public bool IsResultReady(Guid requestKey)
        {
            var requestData = GetRequestData(requestKey);
            return requestData.OutputFileId is not null;
        }

        private ConvertRequest GetRequestData(Guid requestKey)
        {
            var requestData = _dbCtx.ConvertRequests.Find(requestKey);
            if (requestData is null)
                throw new ApplicationException($"Cannot find request data for id {requestKey}");

            return requestData;
        }

        private WorkerStorageFile GetFileData(int key)
        {
            var res = _dbCtx.WorkerStorageFiles.Find(key);
            if (res is null)
                throw new ApplicationException($"Cannot find file data for id {key}");

            return res;
        }

        public abstract StorageTypes GetStorageType();
        public abstract string GetFileUrl(WorkerStorageFile storedFile);
        private protected abstract void DeleteFileContent(string fileKey);
        private protected abstract Stream LoadFileContent(string fileKey);
        private protected abstract Task<string> SaveFileContent(Stream dataStream, string? fileExtension);
        private protected abstract Task<string> SaveFileContent(string sourceFilePath, string? fileExtension);

    }
}
