using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter.Domain.Entities.Exceptions
{
    public class RequestIsMissingException : CriticalConverterException
    {
        public override string Message { get; }

        public RequestIsMissingException(Guid requestGuid)
        {
            Message = $"Request with guid {requestGuid} is missing";
        }
    }
}
