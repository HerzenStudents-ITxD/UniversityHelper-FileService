using System;
using UniversityHelper.FileService.Mappers.Models.Interfaces;
using UniversityHelper.FileService.Models.Dto.Models;

namespace UniversityHelper.FileService.Mappers.Models
{
  public class FileInfoMapper : IFileInfoMapper
  {
    public FileInfo Map(string path, string name, string extension)
    {
      return new FileInfo
      {
        Path = path,
        Name = name,
        Extension = extension
      };
    }
  }
}
