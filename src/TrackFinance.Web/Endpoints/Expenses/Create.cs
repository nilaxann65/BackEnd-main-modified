using System.Net;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TrackFinance.Core.TransactionAgregate;
using TrackFinance.Core.TransactionAgregate.Enum;
using TrackFinance.SharedKernel.Interfaces;

namespace TrackFinance.Web.Endpoints.Expenses;

public class CreateExpenses : EndpointBaseAsync
    .WithRequest<CreateExpenseRequest>
    .WithActionResult<CreateExpenseResponse>
{
    private readonly IRepository<Transaction> _repository;

    public CreateExpenses(IRepository<Transaction> repository)
    {
        _repository = repository;
    }

    [HttpPost("/Expenses")]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Creates a new expense",
        Description = "Creates a new expense",
        OperationId = "Expenses.Create",
        Tags = new[] { "ExpensesEndpoints" })
    ]
    public override async Task<ActionResult<CreateExpenseResponse>> HandleAsync(CreateExpenseRequest request, CancellationToken cancellationToken = default)
    {
        var newExpense = new Transaction(request.Description,
                                         request.Amount,
                                         request.ExpenseType,
                                         request.ExpenseDate,
                                         request.UserId,
                                         TransactionType.Expense);
        var createdItem = await _repository.AddAsync(newExpense, cancellationToken);

        var response = new CreateExpenseResponse(
            statusResult: (int)HttpStatusCode.OK,
            expensesId: createdItem.Id
        );

        return Ok(response);
    }
}
