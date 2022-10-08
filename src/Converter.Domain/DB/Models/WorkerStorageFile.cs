using System.ComponentModel.DataAnnotations.Schema;

namespace Converter.Domain.DB.Models
{
    /// <summary>
    /// информация о файлах, размещаемых в хранилище
    /// </summary>
    [Table("WorkerStorageFiles", Schema = "CNVRT")]
    public class WorkerStorageFile
    {
        public int Id { get; set; }
        
        public string StorageKey { get; set; } = default!;
        
        public StorageTypes StorageType { get; set; }
        
        public FileTypes FileType { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;
    }

    public enum StorageTypes { Disk }

    public enum FileTypes { Input, Output }
}
