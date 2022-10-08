using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter.Domain.Services.Factories
{
    public interface IRequestDataFactory
    {
        IRequestData GetRequestHandler();
    }
}
