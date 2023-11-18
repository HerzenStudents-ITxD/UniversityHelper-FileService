using UniversityHelper.Models.Broker.Common;
using UniversityHelper.Core.BrokerSupport.Attributes;
using UniversityHelper.Core.BrokerSupport.Configurations;
using UniversityHelper.Models.Broker.Requests.Project;

namespace UniversityHelper.FileService.Models.Dto.Configurations
{
  public class RabbitMqConfig : BaseRabbitMqConfig
  {
    public string RemoveFilesEndpoint { get; set; }
    public string GetFilesEndpoint { get; set; }

    // project

    [AutoInjectRequest(typeof(ICheckProjectFilesAccessesRequest))]
    public string CheckFilesAccessesEndpoint { get; set; }

    [AutoInjectRequest(typeof(IGetProjectsUsersRequest))]
    public string GetProjectsUsersEndpoint { get; set; }

    [AutoInjectRequest(typeof(IGetProjectUserRoleRequest))]
    public string GetProjectUserRoleEndpoint { get; set; }

    // wiki

    [AutoInjectRequest(typeof(ICheckArticlesExistence))]
    public string CheckArticlesExistenceEndpoint { get; set; }
  }
}
