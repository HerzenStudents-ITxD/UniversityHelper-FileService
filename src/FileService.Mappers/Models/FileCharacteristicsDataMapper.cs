using System;
using UniversityHelper.FileService.Mappers.Models.Interfaces;
using UniversityHelper.Models.Broker.Models.File;

namespace UniversityHelper.FileService.Mappers.Models
{
  public class FileCharacteristicsDataMapper : IFileCharacteristicsDataMapper
  {
    public FileCharacteristicsData Map(Guid id, string name, string extension, long size, DateTime createdAtUtc)
    {
      return new(
        id: id,
        name: name,
        extension: extension,
        size: size,
        createdAtUtc: createdAtUtc);
    }
  }
}
