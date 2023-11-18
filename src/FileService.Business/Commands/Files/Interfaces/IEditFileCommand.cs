using System;
using System.Threading.Tasks;
using UniversityHelper.Models.Broker.Enums;
using UniversityHelper.Core.Attributes;
using UniversityHelper.Core.Responses;

namespace UniversityHelper.FileService.Business.Commands.Files.Interfaces
{
  [AutoInject]
  public interface IEditFileCommand
  {
    Task<OperationResultResponse<bool>> ExecuteAsync(Guid entityId, Guid fileId, FileSource fileSource, string newName);
  }
}
