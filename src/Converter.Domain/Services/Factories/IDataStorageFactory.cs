using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Converter.Domain.DB.Models;

namespace Converter.Domain.Services.Factories
{
    public interface IDataStorageFactory
    {
        (IDataStorage, WorkerStorageFile) GetDataStorageImplementation(int fileId);
    }
}
