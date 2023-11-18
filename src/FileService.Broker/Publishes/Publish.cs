using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityHelper.Models.Broker.Enums;
using UniversityHelper.Models.Broker.Publishing.Subscriber.File;
using MassTransit;
using UniversityHelper.FileService.Broker.Publishes.Interfaces;
using UniversityHelper.Models.Broker.Publishing.Subscriber.File;

namespace UniversityHelper.FileService.Broker.Publishes
{
  public class Publish : IPublish
  {
    private readonly IBus _bus;

    public Publish(IBus bus)
    {
      _bus = bus;
    }

    public Task CreateProjectFilesAsync(Guid entityId, FileAccessType access, List<Guid> filesIds)
    {
      return _bus.Publish<ICreateProjectFilesPublish>(ICreateProjectFilesPublish.CreateObj(
        filesIds: filesIds,
        access: access,
        projectId: entityId));
    }

    public Task CreateWikiFilesAsync(Guid entityId, List<Guid> filesIds)
    {
      return _bus.Publish<ICreateWikiFilesPublish> (ICreateWikiFilesPublish.CreateObj(
        filesIds: filesIds,
        articleId: entityId));
    }
  }
}
