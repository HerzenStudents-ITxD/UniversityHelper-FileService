using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityHelper.Models.Broker.Enums;
using UniversityHelper.Core.Attributes;
using UniversityHelper.Core.Responses;
using UniversityHelper.Models.Broker.Enums;
using Microsoft.AspNetCore.Http;

namespace UniversityHelper.FileService.Business.Commands.Files.Interfaces
{
  [AutoInject]
  public interface ICreateFilesCommand
  {
    Task<OperationResultResponse<List<Guid>>> ExecuteAsync(Guid entityId, FileSource fileSource, FileAccessType access, IFormFileCollection uploadedFile);
  }
}
