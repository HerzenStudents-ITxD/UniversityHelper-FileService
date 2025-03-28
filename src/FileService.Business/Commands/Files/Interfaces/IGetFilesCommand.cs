﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityHelper.Models.Broker.Enums;
using UniversityHelper.Core.Attributes;

namespace UniversityHelper.FileService.Business.Commands.Files.Interfaces
{
  [AutoInject]
  public interface IGetFilesCommand
  {
    Task<List<(byte[] content, string extension, string name)>> ExecuteAsync(List<Guid> filesIds, FileSource fileSource);
  }
}
