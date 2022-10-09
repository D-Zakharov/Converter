using System.ComponentModel;
using System.Reflection.Metadata;
using Converter.Domain.DB.Models;

namespace Converter.Domain.Services
{
    /// <summary>
    /// Хранение входящих файлов и результата конвертации
    /// </summary>
    public interface IDataStorage
    {
        /// <summary>
        /// Запись в хранилище
        /// </summary>
        /// <param name="fileType">Тип файла (входящий, результат...)</param>
        /// <param name="fileStream">Файл, который необходимо сохранить</param>
        /// <param name="fileExtension">Расширение файла (с точкой)</param>
        /// <returns>Ключ, который в дальнейшем можно будет использовать для чтения файла</returns>
        Task<int> SaveFile(FileTypes fileType, Stream fileStream, string? fileExtension = null);

        /// <summary>
        /// Запись в хранилище
        /// </summary>
        /// <param name="fileType">Тип файла (входящий, результат...)</param>
        /// <param name="sourceFilePath">Путь к контенту, этот исходник будет автоматически удален</param>
        /// <param name="fileExtension">Расширение файла (с точкой)</param>
        /// <returns>Ключ, который в дальнейшем можно будет использовать для чтения файла</returns>
        Task<int> SaveFile(FileTypes fileType, string sourceFilePath, string? fileExtension = null);

        /// <summary>
        /// Чтение из хранилища
        /// </summary>
        /// <param name="key">Ключ файла</param>
        /// <returns></returns>
        Stream LoadFile(int key);

        /// <summary>
        /// Удаление файла из хранилища
        /// </summary>
        /// <param name="key">идентификатор файла</param>
        void DeleteFile(int key);

        /// <summary>
        /// Удаление файла из хранилища
        /// </summary>
        /// <param name="file">вся информация по файлу из бд</param>
        void DeleteFile(WorkerStorageFile file);

        /// <summary>
        /// Возвращает тип хранилища, значение хранится в БД для последующего сопоставления с обработчиком
        /// </summary>
        /// <returns></returns>
        StorageTypes GetStorageType();

        /// <summary>
        /// Получить файл c результатом по ключу запроса
        /// </summary>
        /// <param name="requestKey">Клюс запроса</param>
        /// <returns></returns>
        Stream? LoadResultFileByRequestGuid(Guid requestKey);

        /// <summary>
        /// Получить Url файла по ключу. Это требуется для puppeteer
        /// </summary>
        /// <param name="storedFile">информация о файле</param>
        /// <returns></returns>
        string GetFileUrl(WorkerStorageFile storedFile);
        
        /// <summary>
        /// Проверяет, готов ли результат
        /// </summary>
        /// <param name="requestKey">Ключ запроса</param>
        /// <returns></returns>
        bool IsResultReady(Guid requestKey);
    }
}
