﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityHelper.Models.Broker.Common;
using UniversityHelper.FileService.Broker.Requests.Interfaces;
using UniversityHelper.Core.BrokerSupport.Helpers;
using UniversityHelper.Models.Broker.Common;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace UniversityHelper.FileService.Broker.Requests
{
  public class WikiService : IWikiService
  {
    private readonly ILogger<WikiService> _logger;
    private readonly IRequestClient<ICheckArticlesExistence> _rcCheckArticles;

    public WikiService(
      ILogger<WikiService> logger,
      IRequestClient<ICheckArticlesExistence> rcCheckArticles)
    {
      _logger = logger;
      _rcCheckArticles = rcCheckArticles;
    }

    public async Task<List<Guid>> CheckArticlesAsync(List<Guid> atriclesIds, List<string> errors = null)
    {
      if (atriclesIds is null || !atriclesIds.Any())
      {
        return null;
      }

      return (await RequestHandler
        .ProcessRequest<ICheckArticlesExistence, ICheckArticlesExistence>(
          _rcCheckArticles,
          ICheckArticlesExistence.CreateObj(atriclesIds),
          errors,
          _logger))?.ArticlesIds;
    }
  }
}
