using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TrackFinance.Core.TransactionAgregate;
using TrackFinance.Core.TransactionAgregate.Enum;
using TrackFinance.SharedKernel.Interfaces;

namespace TrackFinance.Web.Endpoints.Expenses;

public class Update : EndpointBaseAsync
    .WithRequest<UpdateExpenseRequest>
    .WithActionResult<UpdateExpenseResponse>
{
    private readonly IRepository<Transaction> _repository;

    public Update(IRepository<Transaction> repository)
    {
        _repository = repository;
    }

    [HttpPut(UpdateExpenseRequest.Route)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Updates an expense",
        Description = "Updates an expense",
        OperationId = "Expense.Update",
        Tags = new[] { "ExpensesEndpoints" })
    ]
    public override async Task<ActionResult<UpdateExpenseResponse>> HandleAsync(UpdateExpenseRequest request, CancellationToken cancellationToken = default)
    {
        var existingExpense = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (existingExpense == null)
        {
            return NotFound();
        }

        existingExpense.UpdateValue(request.Description, request.Amount, request.ExpenseType, request.ExpenseDate, request.UserId, TransactionType.Expense);

        await _repository.UpdateAsync(existingExpense, cancellationToken);

        var response = new UpdateExpenseResponse(
            expenseRecord: new ExpenseRecord(
                existingExpense.Id,
                existingExpense.Description,
                existingExpense.Amount,
                existingExpense.TransactionDescriptionType,
                existingExpense.ExpenseDate,
                existingExpense.UserId,
                existingExpense.TransactionType));

        return Ok(response);
    }
}
