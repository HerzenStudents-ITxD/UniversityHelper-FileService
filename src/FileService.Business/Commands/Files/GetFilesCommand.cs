using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityHelper.FileService.Broker.Requests.Interfaces;
using UniversityHelper.FileService.Data.Interfaces;
using UniversityHelper.FileService.Mappers.Models.Interfaces;
using UniversityHelper.Core.BrokerSupport.AccessValidatorEngine.Interfaces;
using UniversityHelper.Core.Constants;
using FileInfo = UniversityHelper.FileService.Models.Dto.Models.FileInfo;
using UniversityHelper.FileService.Business.Commands.Files.Interfaces;
using System.IO;
using UniversityHelper.Models.Broker.Enums;

namespace UniversityHelper.FileService.Business.Commands.Files
{
  public class GetFilesCommand : IGetFilesCommand
  {
    private readonly IFileRepository _repository;
    private readonly IProjectService _projectService;
    private readonly IAccessValidator _accessValidator;
    private readonly IContentTypeMapper _mapper;

    public GetFilesCommand(
      IFileRepository repository,
      IProjectService projectService,
      IAccessValidator accessValidator,
      IContentTypeMapper mapper)
    {
      _repository = repository;
      _projectService = projectService;
      _accessValidator = accessValidator;
      _mapper = mapper;
    }

    public async Task<List<(byte[] content, string extension, string name)>> ExecuteAsync(List<Guid> filesIds, FileSource fileSource)
    {
      if (filesIds is null)
      {
        return null;
      }

      if (fileSource == FileSource.Project)
      {
        if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveProjects))
        {
          filesIds = await _projectService.CheckFilesAsync(filesIds);
        }
      }

      List<(byte[] content, string extension, string name)> result = new();
      List<FileInfo> files = await _repository.GetFileInfoAsync(fileSource, filesIds);

      foreach (FileInfo fileInfo in files)
      {
        result.Add((await File.ReadAllBytesAsync(fileInfo.Path), _mapper.Map(fileInfo.Extension), fileInfo.Name));
      }

      return result;
    }
  }
}
