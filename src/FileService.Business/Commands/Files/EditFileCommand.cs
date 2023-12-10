using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UniversityHelper.Models.Broker.Enums;
using UniversityHelper.FileService.Broker.Requests.Interfaces;
using UniversityHelper.FileService.Business.Commands.Files.Interfaces;
using UniversityHelper.FileService.Data.Interfaces;
using UniversityHelper.Core.BrokerSupport.AccessValidatorEngine.Interfaces;
using UniversityHelper.Core.Constants;
using UniversityHelper.Core.Extensions;
using UniversityHelper.Core.Helpers.Interfaces;
using UniversityHelper.Core.Responses;
using UniversityHelper.Models.Broker.Enums;
using Microsoft.AspNetCore.Http;

namespace UniversityHelper.FileService.Business.Commands.Files
{
  public class EditFileCommand : IEditFileCommand
  {
    private readonly IAccessValidator _accessValidator;
    private readonly IFileRepository _fileRepository;
    private readonly IResponseCreator _responseCreator;
    private readonly IProjectService _projectService;
    private readonly IWikiService _wikiService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EditFileCommand(
      IAccessValidator accessValidator,
      IResponseCreator responseCreator,
      IFileRepository fileRepository,
      IProjectService projectService,
      IWikiService wikiService,
      IHttpContextAccessor httpContextAccessor)
    {
      _accessValidator = accessValidator;
      _fileRepository = fileRepository;
      _responseCreator = responseCreator;
      _projectService = projectService;
      _wikiService = wikiService;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<OperationResultResponse<bool>> ExecuteAsync(Guid entityId, Guid fileId, FileSource fileSource, string newName)
    {
      // TODO Rework
      //if (fileSource == FileSource.Project)
      //{
      //  (ProjectStatusType projectStatus, ProjectUserRoleType? projectUserRole) = await _projectService.GetProjectUserRole(entityId, _httpContextAccessor.HttpContext.GetUserId());
      //  if (!projectStatus.Equals(ProjectStatusType.Active)
      //    || !(projectUserRole.HasValue && projectUserRole.Value.Equals(ProjectUserRoleType.Manager))
      //    && !await _accessValidator.HasRightsAsync(Rights.AddEditRemoveProjects))
      //  {
      //    return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
      //  }
      //}
      //else if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveWiki)
      //  || !_wikiService.CheckArticlesAsync(new List<Guid> { entityId }).Result.Any())
      //{
      //  return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
      //}

      OperationResultResponse<bool> response = new(
        body: await _fileRepository.EditNameAsync(fileSource, fileId, newName));

      if (!response.Body)
      {
        response = _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.BadRequest);
      }

      return response;
    }
  }
}
