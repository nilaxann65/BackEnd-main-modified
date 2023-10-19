using System.Net.Mime;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TrackFinance.Core.Interfaces;
using TrackFinance.Core.TransactionAgregate;
using TrackFinance.SharedKernel.Interfaces;

namespace TrackFinance.Web.Endpoints.Historical;

public class GetHistoricalRecordByUser : EndpointBaseAsync
  .WithRequest<GetHistoricalRecordByUserRequest>
  .WithActionResult<GetHistoricalRecordByUserResponse>
{
  private readonly IRepository<Transaction> _repository;
  private readonly IHistoricCacheContextService _cacheRepository;

  public GetHistoricalRecordByUser(IRepository<Transaction> repository, IHistoricCacheContextService cacheRepository)
  {
    _repository = repository;
    _cacheRepository = cacheRepository;
  }

  [Produces(MediaTypeNames.Application.Json)]
  [HttpGet(GetHistoricalRecordByUserRequest.Route)]
  [SwaggerOperation(
      Summary = "Get Historical Records by user",
      Description = "Get Historical Records by userId",
      OperationId = "HistoricalRecords.GetHistoricalRecordByUser",
      Tags = new[] { "HistoricalRecordsEndpoints" })
  ]
  public override async Task<ActionResult<GetHistoricalRecordByUserResponse>> HandleAsync([FromRoute] GetHistoricalRecordByUserRequest request, CancellationToken cancellationToken = default)
  {
    if (_cacheRepository.existCache(request.userId.ToString()))
      return Ok(new GetHistoricalRecordByUserResponse
      {
        HistoricalRecord = (List<HistoricalRecord>)_cacheRepository.getCache(request.userId.ToString())
      });


    var data = (await _repository.ListAsync(cancellationToken))
   .Where(expense => expense.UserId == request.userId)
   .Select(expense => new HistoricalRecord(
                                         description: expense.Description,
                                         transactionDescriptionType: expense.TransactionDescriptionType,
                                         amount: expense.Amount,
                                         expenseDate: expense.ExpenseDate,
                                         transactionType: expense.TransactionType))
   .OrderBy(d => d.expenseDate)
   .ToList();

    _cacheRepository.setCache(request.userId.ToString(), data);
    return Ok(new GetHistoricalRecordByUserResponse
    {
      HistoricalRecord = data
    });
  }
}
