using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter.ConverterService.Services.Converters
{
    public interface IConverter
    {
        public Task<string> Convert(string inputFilePath);
    }
}
