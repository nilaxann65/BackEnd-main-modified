using System.Net;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TrackFinance.Core.TransactionAgregate;
using TrackFinance.Core.TransactionAgregate.Enum;
using TrackFinance.SharedKernel.Interfaces;
using TrackFinance.Web.Endpoints.Expense;


namespace TrackFinance.Web.Endpoints.Incomes;

public class Create : EndpointBaseAsync
    .WithRequest<CreateIncomesRequest>
    .WithActionResult<CreateIncomesResponse>
{
  private readonly IRepository<Transaction> _repository;

  public Create(IRepository<Transaction> repository)
  {
    _repository = repository;
  }

  [HttpPost("/Incomes")]
  [Produces("application/json")]
  [SwaggerOperation(
    Summary = "Creates a new Incomes",
    Description = "Creates a new Incomes",
    OperationId = "Incomes.Create",
    Tags = new[] { "IncomesEndpoints" })
  ]
  public override async Task<ActionResult<CreateIncomesResponse>> HandleAsync(CreateIncomesRequest request, CancellationToken cancellationToken = default)
  {
    var newExpense = new Transaction(request.Description,
                                     request.Amount,
                                     request.ExpenseType,
                                     request.ExpenseDate,
                                     request.UserId,
                                     TransactionType.Income);
    var createdItem = await _repository.AddAsync(newExpense, cancellationToken);

    var response = new CreateIncomesResponse
    (
    statusResult: (int)HttpStatusCode.OK,
    expensesId: createdItem.Id
    );

    return Ok(response);
  }
}
