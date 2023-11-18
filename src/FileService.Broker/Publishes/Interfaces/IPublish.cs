using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityHelper.Core.Attributes;
using UniversityHelper.Models.Broker.Enums;

namespace UniversityHelper.FileService.Broker.Publishes.Interfaces
{
  [AutoInject]
  public interface IPublish
  {
    Task CreateProjectFilesAsync(Guid entityId, FileAccessType access, List<Guid> filesIds);

    Task CreateWikiFilesAsync(Guid entityId, List<Guid> filesIds);
  }
}
