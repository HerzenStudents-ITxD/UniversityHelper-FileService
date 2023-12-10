using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UniversityHelper.Models.Broker.Enums;
using UniversityHelper.FileService.Broker.Publishes.Interfaces;
using UniversityHelper.FileService.Broker.Requests.Interfaces;
using UniversityHelper.FileService.Business.Commands.Files.Interfaces;
using UniversityHelper.FileService.Data.Interfaces;
using UniversityHelper.FileService.Mappers.Db.Interfaces;
using UniversityHelper.FileService.Models.Db;
using UniversityHelper.FileService.Validation.Interfaces;
using UniversityHelper.Core.BrokerSupport.AccessValidatorEngine.Interfaces;
using UniversityHelper.Core.Constants;
using UniversityHelper.Core.Exceptions.Models;
using UniversityHelper.Core.Extensions;
using UniversityHelper.Core.Helpers.Interfaces;
using UniversityHelper.Core.Responses;
using UniversityHelper.Models.Broker.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using FluentValidation.Results;

namespace UniversityHelper.FileService.Business.Commands.Files
{
  public class CreateFilesCommand : ICreateFilesCommand
  {
    private readonly IFileRepository _fileRepository;
    private readonly IResponseCreator _responseCreator;
    private readonly IAccessValidator _accessValidator;
    private readonly IProjectService _projectService;
    private readonly IWikiService _wikiService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPublish _publish;
    private readonly IDbFileMapper _mapper;
    private readonly IWebHostEnvironment _appEnvironment;
    private readonly IAddFileRequestValidator _validator;

    public CreateFilesCommand(
      IResponseCreator responseCreator,
      IFileRepository fileRepository,
      IAccessValidator accessValidator,
      IProjectService projectService,
      IWikiService wikiService,
      IHttpContextAccessor httpContextAccessor,
      IPublish publish,
      IDbFileMapper mapper,
      IWebHostEnvironment appEnvironment)
    {
      _fileRepository = fileRepository;
      _responseCreator = responseCreator;
      _accessValidator = accessValidator;
      _projectService = projectService;
      _wikiService = wikiService;
      _httpContextAccessor = httpContextAccessor;
      _publish = publish;
      _mapper = mapper;
      _appEnvironment = appEnvironment;
    }

    public async Task<OperationResultResponse<List<Guid>>> ExecuteAsync(
      Guid entityId,
      FileSource fileSource,
      FileAccessType access,
      IFormFileCollection uploadedFiles)
    {
      // TODO Rework
      //if (fileSource == FileSource.Project)
      //{
      //  (ProjectStatusType projectStatus, ProjectUserRoleType? projectUserRole) = await _projectService.GetProjectUserRole(entityId, _httpContextAccessor.HttpContext.GetUserId());
      //  if (!projectStatus.Equals(ProjectStatusType.Active)
      //    || !(projectUserRole.HasValue && projectUserRole.Value.Equals(ProjectUserRoleType.Manager))
      //    && !await _accessValidator.HasRightsAsync(Rights.AddEditRemoveProjects))
      //  {
      //    return _responseCreator.CreateFailureResponse<List<Guid>>(HttpStatusCode.Forbidden);
      //  }
      //}
      //else if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveWiki)
      //  || !_wikiService.CheckArticlesAsync(new List<Guid> { entityId }).Result.Any())
      //{
      //  return _responseCreator.CreateFailureResponse<List<Guid>>(HttpStatusCode.Forbidden);
      //}

      ValidationResult validationResult = await _validator.ValidateAsync(uploadedFiles);
      if (!validationResult.IsValid)
      {
        throw new BadRequestException(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
      }

      string uploadPath = $"{Directory.GetCurrentDirectory()}/uploads";
      Directory.CreateDirectory(uploadPath);
      List<DbFile> files = new();

      foreach (IFormFile uploadedFile in uploadedFiles)
      {
        string fullPath = $"{uploadPath}/{Guid.NewGuid()}_{uploadedFile.FileName}";
        using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
        {
          await uploadedFile.CopyToAsync(fileStream);
        }

        files.Add(_mapper.Map(uploadedFile, fullPath));
      }

      OperationResultResponse<List<Guid>> response = new(body: await _fileRepository.
        CreateAsync(fileSource, files));

      if (response.Body.Any())
      {
        switch (fileSource)
        {
          case FileSource.Project:
            await _publish.CreateProjectFilesAsync(entityId, access, response.Body);
            break;
          case FileSource.Wiki:
            await _publish.CreateWikiFilesAsync(entityId, response.Body);
            break;
        }

        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
      }
      else
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
      }

      return response;
    }
  }
}
