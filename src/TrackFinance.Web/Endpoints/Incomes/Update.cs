using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TrackFinance.Core.TransactionAgregate;
using TrackFinance.Core.TransactionAgregate.Enum;
using TrackFinance.SharedKernel.Interfaces;

namespace TrackFinance.Web.Endpoints.Incomes;

public class Update : EndpointBaseAsync
    .WithRequest<UpdateIncomeRequest>
    .WithActionResult<UpdateIncomesResponse>
{
  private readonly IRepository<Transaction> _repository;

  public Update(IRepository<Transaction> repository)
  {
    _repository = repository;
  }

  [HttpPut(UpdateIncomeRequest.Route)]
  [Produces("application/json")]
  [SwaggerOperation(
     Summary = "Updates a incomes",
     Description = "Updates a incomes",
     OperationId = "Income.Update",
     Tags = new[] { "IncomesEndpoints" })
  ]
  public override async Task<ActionResult<UpdateIncomesResponse>> HandleAsync(UpdateIncomeRequest request, CancellationToken cancellationToken = default)
  {
    var existingIncomes = await _repository.GetByIdAsync(request.Id, cancellationToken);

    if (existingIncomes == null)
    {
      return NotFound();
    }

    existingIncomes.UpdateValue(request.Description, request.Amount, request.ExpenseType, request.ExpenseDate, request.UserId, TransactionType.Income);

    await _repository.UpdateAsync(existingIncomes, cancellationToken);

    var response = new UpdateIncomesResponse(
        expenseRecord: new IncomeRecord(
          existingIncomes.Id,
          existingIncomes.Description,
          existingIncomes.Amount,
          existingIncomes.TransactionDescriptionType,
          existingIncomes.ExpenseDate,
          existingIncomes.UserId,
          existingIncomes.TransactionType));

    return Ok(response);
  }
}
