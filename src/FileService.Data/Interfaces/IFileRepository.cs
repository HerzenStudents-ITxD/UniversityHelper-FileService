using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityHelper.Models.Broker.Enums;
using UniversityHelper.FileService.Models.Db;
using UniversityHelper.FileService.Models.Dto.Models;
using UniversityHelper.Core.Attributes;
using UniversityHelper.Models.Broker.Models.File;

namespace UniversityHelper.FileService.Data.Interfaces
{
  [AutoInject]
  public interface IFileRepository
  {
    Task<List<Guid>> CreateAsync(FileSource fileSource, List<DbFile> files);

    Task<List<FileInfo>> GetFileInfoAsync(FileSource fileSource, List<Guid> filesIds);

    Task<List<FileCharacteristicsData>> GetFileCharacteristicsDataAsync(FileSource fileSource, List<Guid> filesIds);

    Task<List<string>> RemoveAsync(FileSource fileSource, List<Guid> filesIds);

    Task<bool> EditNameAsync(FileSource fileSource, Guid fileId, string newName);
  }
}
