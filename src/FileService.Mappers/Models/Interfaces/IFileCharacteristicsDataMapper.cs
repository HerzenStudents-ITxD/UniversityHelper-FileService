using System;
using UniversityHelper.Core.Attributes;
using UniversityHelper.Models.Broker.Models.File;

namespace UniversityHelper.FileService.Mappers.Models.Interfaces
{
  [AutoInject]
  public interface IFileCharacteristicsDataMapper
  {
    FileCharacteristicsData Map(Guid id, string name, string extension, long size, DateTime createdAtUtc);
  }
}
