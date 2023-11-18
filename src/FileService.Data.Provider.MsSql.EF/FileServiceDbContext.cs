using System.Linq;
using System.Threading.Tasks;
using UniversityHelper.FileService.Models.Db;
using UniversityHelper.Core.EFSupport.Provider;
using Microsoft.EntityFrameworkCore;

namespace UniversityHelper.FileService.Data.Provider.MsSql.Ef
{
  /// <summary>
  /// A class that defines the tables and its properties in the database of FileService.
  /// </summary>
  public class FileServiceDbContext : DbContext, IDataProvider
  {
    public FileServiceDbContext(DbContextOptions<FileServiceDbContext> options)
      : base(options)
    {
    }

    public DbSet<DbFile> Files { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }

    void IBaseDataProvider.Save()
    {
      SaveChanges();
    }

    public void EnsureDeleted()
    {
      Database.EnsureDeleted();
    }

    public bool IsInMemory()
    {
      return Database.IsInMemory();
    }

    public object MakeEntityDetached(object obj)
    {
      Entry(obj).State = EntityState.Detached;

      return Entry(obj).State;
    }

    public async Task SaveAsync()
    {
      await SaveChangesAsync();
    }

    public async Task<int> ExecuteRawSqlAsync(string query)
    {
      return await Database.ExecuteSqlRawAsync(query);
    }

    public IQueryable<DbFile> FromSqlRaw(string query)
    {
      return Files.FromSqlRaw(query);
    }
  }
}
