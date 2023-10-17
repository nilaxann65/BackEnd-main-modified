using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TrackFinance.Core.TransactionAgregate;
using TrackFinance.SharedKernel.Interfaces;

namespace TrackFinance.Web.Endpoints.Incomes;

public class Delete : EndpointBaseAsync
    .WithRequest<DeleteIncomeRequest>
    .WithoutResult
{
  private readonly IRepository<Transaction> _repository;

  public Delete(IRepository<Transaction> repository)
  {
    _repository = repository;
  }

  [HttpDelete(DeleteIncomeRequest.Route)]
  [SwaggerOperation(
      Summary = "Deletes a Incomes",
      Description = "Delete a Income Saved",
      OperationId = "Income.Delete",
      Tags = new[] { "IncomesEndpoints" })
  ]
  public override async Task<ActionResult> HandleAsync([FromRoute] DeleteIncomeRequest request, CancellationToken cancellationToken = default)
  {
    var aggregateToDelete = await _repository.GetByIdAsync(request.IncomeId);
    if (aggregateToDelete == null) return NotFound();

    await _repository.DeleteAsync(aggregateToDelete, cancellationToken);

    return NoContent();
  }
}
