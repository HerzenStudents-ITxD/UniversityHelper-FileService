using UniversityHelper.FileService.Models.Db;
using UniversityHelper.Core.Attributes;
using Microsoft.AspNetCore.Http;

namespace UniversityHelper.FileService.Mappers.Db.Interfaces
{
  [AutoInject]
  public interface IDbFileMapper
  {
    DbFile Map(IFormFile uploadedFile, string path);
  }
}
