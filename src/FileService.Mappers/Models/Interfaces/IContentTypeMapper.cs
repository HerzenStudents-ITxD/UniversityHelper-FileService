using UniversityHelper.Core.Attributes;

namespace UniversityHelper.FileService.Mappers.Models.Interfaces
{
  [AutoInject]
  public interface IContentTypeMapper
  {
    string Map(string extension);
  }
}
