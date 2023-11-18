using System;
using System.IO;
using UniversityHelper.FileService.Mappers.Db.Interfaces;
using UniversityHelper.FileService.Models.Db;
using UniversityHelper.Core.Extensions;
using Microsoft.AspNetCore.Http;
using MimeTypes;

namespace UniversityHelper.FileService.Mappers.Db
{
  public class DbFileMapper : IDbFileMapper
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DbFileMapper(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    public DbFile Map(IFormFile uploadedFile, string path)
    {
      if (uploadedFile is null)
      {
        return null;
      }

      using MemoryStream ms = new();
      uploadedFile.CopyTo(ms);

      return new()
      {
        Id = Guid.NewGuid(),
        Extension = MimeTypeMap.GetExtension(uploadedFile.ContentType),
        Name = Path.GetFileNameWithoutExtension(uploadedFile.FileName),
        Size = uploadedFile.Length,
        CreatedBy = _httpContextAccessor.HttpContext.GetUserId(),
        CreatedAtUtc = DateTime.UtcNow,
        Path = path
      };
    }
  }
}
