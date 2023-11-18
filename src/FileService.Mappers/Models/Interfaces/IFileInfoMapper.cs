using System;
using UniversityHelper.FileService.Models.Dto.Models;
using UniversityHelper.Core.Attributes;

namespace UniversityHelper.FileService.Mappers.Models.Interfaces
{
  [AutoInject]
  public interface IFileInfoMapper
  {
    FileInfo Map(string path, string name, string extension);
  }
}
