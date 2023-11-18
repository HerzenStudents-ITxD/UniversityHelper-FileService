using UniversityHelper.Core.Attributes;
using UniversityHelper.Models.Broker.Enums;
using UniversityHelper.Models.Broker.Models.Project;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UniversityHelper.FileService.Broker.Requests.Interfaces
{
  [AutoInject]
  public interface IProjectService
  {
    Task<List<Guid>> CheckFilesAsync(List<Guid> filesIds, List<string> errors = null);

    Task<List<ProjectUserData>> GetProjectUsersAsync(List<Guid> usersIds);

    Task<(ProjectStatusType projectStatus, ProjectUserRoleType? projectUserRole)> GetProjectUserRole(Guid projectId, Guid userId);
  }
}
