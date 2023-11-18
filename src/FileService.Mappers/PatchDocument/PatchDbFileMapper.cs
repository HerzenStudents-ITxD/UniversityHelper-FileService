using System;
using UniversityHelper.FileService.Mappers.PatchDocument.Interfaces;
using UniversityHelper.FileService.Models.Db;
using UniversityHelper.FileService.Models.Dto.Requests;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace UniversityHelper.FileService.Mappers.PatchDocument
{
  public class PatchDbFileMapper : IPatchDbFileMapper
  {
    public JsonPatchDocument<DbFile> Map(JsonPatchDocument<EditFileRequest> request)
    {
      if (request == null)
      {
        return null;
      }

      JsonPatchDocument<DbFile> dbRequest = new();

      foreach (var item in request.Operations)
      {
        dbRequest.Operations.Add(new Operation<DbFile>(item.op, item.path, item.from, item.value));
      }

      return dbRequest;
    }
  }
}
