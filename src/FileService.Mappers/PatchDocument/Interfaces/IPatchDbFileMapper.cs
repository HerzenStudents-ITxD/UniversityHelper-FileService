using UniversityHelper.FileService.Models.Db;
using UniversityHelper.FileService.Models.Dto.Requests;
using UniversityHelper.Core.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace UniversityHelper.FileService.Mappers.PatchDocument.Interfaces
{
  [AutoInject]
  public interface IPatchDbFileMapper
  {
    JsonPatchDocument<DbFile> Map(JsonPatchDocument<EditFileRequest> request);
  }
}
