using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Converter.Domain.DB.Models;
using Microsoft.EntityFrameworkCore;

namespace Converter.Domain.DB
{
    public class ConverterDbContext : DbContext
    {
        public virtual DbSet<ConvertRequest> ConvertRequests { get; set; } = default!;
        public virtual DbSet<WorkerStorageFile> WorkerStorageFiles { get; set; } = default!;

        public ConverterDbContext(DbContextOptions<ConverterDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
