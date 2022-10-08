using Converter.Domain.Config;
using Converter.Domain.DB;
using Converter.Domain.DB.Models;
using Converter.Domain.Entities.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Converter.Domain.Services.Factories
{
    public class DataStorageFactory : IDataStorageFactory
    {
        private readonly IDbContextFactory<ConverterDbContext> _dbContextFactory;
        private readonly IOptions<StorageConfig> _config;

        public DataStorageFactory(IDbContextFactory<ConverterDbContext> dbContextFactory, IOptions<StorageConfig> config)
        {
            _dbContextFactory = dbContextFactory;
            _config = config;
        }

        public IDataStorage GetDataStorageImplementation(StorageTypes storageType)
        {
            switch (storageType)
            {
                case StorageTypes.Disk: return new DiskDataStorage(_dbContextFactory.CreateDbContext(), _config);
            }

            throw new NotImplementedException($"{nameof(DataStorageFactory)} is not implemeted for {storageType}");
        }

        public (IDataStorage, WorkerStorageFile) GetDataStorageImplementation(int fileId)
        {
            var dbCtx = _dbContextFactory.CreateDbContext();
            var fileData = dbCtx.WorkerStorageFiles.Find(fileId);
            if (fileData is null)
                throw new FileIsMissingException(fileId);

            IDataStorage dataStorage = GetDataStorageImplementation(fileData.StorageType);
            return (dataStorage, fileData);
        }
    }
}
