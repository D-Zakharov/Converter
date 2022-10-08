using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter.Domain.DB.Models
{
    [Table("ConvertRequests", Schema = "CNVRT")]
    public class ConvertRequest
    {
        [Key]
        public Guid RequestKey { get; set; }

        public int? InputFileId { get; set; }

        public int? OutputFileId { get; set; }
    }
}
