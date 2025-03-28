﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityHelper.FileService.Broker.Requests.Interfaces;
using UniversityHelper.Core.BrokerSupport.Helpers;
using UniversityHelper.Core.Extensions;
using UniversityHelper.Models.Broker.Enums;
using UniversityHelper.Models.Broker.Models.Project;
using UniversityHelper.Models.Broker.Requests.Project;
using UniversityHelper.Models.Broker.Responses.Project;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace UniversityHelper.FileService.Broker.Requests
{
  public class ProjectService : IProjectService
  {
    private readonly ILogger<ProjectService> _logger;
    private readonly IRequestClient<ICheckProjectFilesAccessesRequest> _rcCheckFiles;
    private readonly IRequestClient<IGetProjectsUsersRequest> _rcGetProjectUser;
    private readonly IRequestClient<IGetProjectUserRoleRequest> _rcCheckProjectExistence;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProjectService(
      ILogger<ProjectService> logger,
      IRequestClient<ICheckProjectFilesAccessesRequest> rcCheckFiles,
      IRequestClient<IGetProjectsUsersRequest> rcGetProjectUser,
      IRequestClient<IGetProjectUserRoleRequest> rcCheckProjectExistence,
      IHttpContextAccessor httpContextAccessor)
    {
      _logger = logger;
      _rcCheckFiles = rcCheckFiles;
      _rcGetProjectUser = rcGetProjectUser;
      _rcCheckProjectExistence = rcCheckProjectExistence;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<Guid>> CheckFilesAsync(List<Guid> filesIds, List<string> errors = null)
    {
      if (filesIds is null || !filesIds.Any())
      {
        return null;
      }

      return await RequestHandler
        .ProcessRequest<ICheckProjectFilesAccessesRequest, List<Guid>>(
          _rcCheckFiles,
          ICheckProjectFilesAccessesRequest.CreateObj(_httpContextAccessor.HttpContext.GetUserId(), filesIds),
          errors,
          _logger);
    }

    public async Task<List<ProjectUserData>> GetProjectUsersAsync(List<Guid> usersIds)
    {
      return (await RequestHandler
        .ProcessRequest<IGetProjectsUsersRequest, IGetProjectsUsersResponse>(
          _rcGetProjectUser,
          IGetProjectsUsersRequest.CreateObj(usersIds: usersIds),
          logger: _logger))?.Users;
    }

    public async Task<(ProjectStatusType projectStatus, ProjectUserRoleType? projectUserRole)> GetProjectUserRole(Guid projectId, Guid userId)
    {
      IGetProjectUserRoleResponse result = (await RequestHandler
        .ProcessRequest<IGetProjectUserRoleRequest, IGetProjectUserRoleResponse>(
          _rcCheckProjectExistence,
          IGetProjectUserRoleRequest.CreateObj(projectId: projectId, userId: userId),
          logger: _logger));

      return (result.ProjectStatus, result.ProjectUserRole);
    }
  }
}
