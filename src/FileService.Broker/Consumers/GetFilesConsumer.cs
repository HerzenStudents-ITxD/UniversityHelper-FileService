using System.Threading.Tasks;
using UniversityHelper.FileService.Data.Interfaces;
using UniversityHelper.Core.BrokerSupport.Broker;
using UniversityHelper.Models.Broker.Requests.File;
using UniversityHelper.Models.Broker.Responses.File;
using MassTransit;

namespace UniversityHelper.FileService.Broker.Consumers
{
  public class GetFilesConsumer : IConsumer<IGetFilesRequest>
  {
    private readonly IFileRepository _repository;

    private async Task<object> GetFilesAsync(IGetFilesRequest request)
    {
      return IGetFilesResponse.CreateObj(await _repository.GetFileCharacteristicsDataAsync(request.FileSource, request.FilesIds));
    }

    public GetFilesConsumer(IFileRepository repository)
    {
      _repository = repository;
    }

    public async Task Consume(ConsumeContext<IGetFilesRequest> context)
    {
      object response = OperationResultWrapper.CreateResponse(GetFilesAsync, context.Message);

      await context.RespondAsync<IOperationResult<IGetFilesResponse>>(response);
    }
  }
}
