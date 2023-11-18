using System.Linq;
using System.Threading.Tasks;
using UniversityHelper.FileService.Models.Db;
using UniversityHelper.Core.Attributes;
using UniversityHelper.Core.EFSupport.Provider;
using UniversityHelper.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace UniversityHelper.FileService.Data.Provider
{
  [AutoInject(InjectType.Scoped)]
  public interface IDataProvider : IBaseDataProvider
  {
    DbSet<DbFile> Files { get; set; }

    Task<int> ExecuteRawSqlAsync(string query);

    IQueryable<DbFile> FromSqlRaw(string query);
  }
}
