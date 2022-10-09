using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Converter.Domain.Config;
using Converter.Domain.DB;
using Converter.Domain.DB.Models;
using Microsoft.Extensions.Options;

namespace Converter.Domain.Services
{
    /// <summary>
    /// хранилище представляет собой папку в файловой системе
    /// </summary>
    public class DiskDataStorage : AbstractDataStorage
    {
        private readonly string _folderPath;

        public DiskDataStorage(ConverterDbContext dbCtx, IOptions<StorageConfig> config) : base(dbCtx)
        {
            _folderPath = config.Value.FilesFolder;
        }

        public override string GetFileUrl(WorkerStorageFile storedFile)
        {
            return $"file:///{storedFile.StorageKey.Replace(Path.DirectorySeparatorChar, '/')}";
        }

        public override StorageTypes GetStorageType()
        {
            return StorageTypes.Disk;
        }

        private protected override void DeleteFileContent(string filePath)
        {
            File.Delete(filePath);
        }

        private protected override Stream LoadFileContent(string filePath)
        {
            return new FileStream(filePath, FileMode.Open);
        }

        private protected override async Task<string> SaveFileContent(Stream dataStream, string? fileExtension)
        {
            string filePath = GenerateFilePath(fileExtension);
            using (var fs = new FileStream(filePath, FileMode.CreateNew))
            {
                await dataStream.CopyToAsync(fs);
            }
            return filePath;
        }

        private protected override async Task<string> SaveFileContent(string sourceFilePath, string? fileExtension)
        {
            string newFilePath = GenerateFilePath(fileExtension);
            await Task.Run(() => File.Move(sourceFilePath, newFilePath));
            return newFilePath;
        }

        private string GenerateFilePath(string? fileExtension)
        {
            return $"{_folderPath}{Path.DirectorySeparatorChar}{Guid.NewGuid()}{fileExtension}";
        }
    }
}
