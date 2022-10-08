using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Converter.Domain.DB.Models;

namespace Converter.Domain.Services;

/// <summary>
/// управление хранением информации о запросе
/// </summary>
public interface IRequestData
{
    ConvertRequest GetInfo(Guid value);
    void RemoveInfo(ConvertRequest requestData);
    ConvertRequest? SaveInfo(Guid requestKey, int? inputFileId = null, int? outputFileId = null);
    void UpdateInfo(ConvertRequest request, int? inputFileId, int? outputFileId);
}
