using Ardalis.ApiEndpoints;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TrackFinance.Core.Interfaces;
using TrackFinance.Core.Services;
using TrackFinance.Core.TransactionAgregate.Enum;

namespace TrackFinance.Web.Endpoints.Balance;

public class GetListByDateRange : EndpointBaseAsync
.WithRequest<GetListByDateRangeRequest>
.WithActionResult<GetListByDateRangeResponse>
{
    private readonly ITransactionFinanceService _transactionService;

    public GetListByDateRange(ITransactionFinanceService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet(GetListByDateRangeRequest.Route)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "group by date range",
        Description = "group by date range",
        OperationId = "Balance.List",
        Tags = new[] { "BalanceEndpoints" })
    ]
    public override async Task<ActionResult<GetListByDateRangeResponse>> HandleAsync([FromRoute] GetListByDateRangeRequest request, CancellationToken cancellationToken = default)
    {
        var transactions = await _transactionService.GetTransactionItemsByDateRangeAsync(request.StartDate, request.EndDate, request.UserId, request.TransactionType, cancellationToken);

        if (transactions.Status != ResultStatus.Ok) return BadRequest(transactions.ValidationErrors);

        return Ok(new GetListByDateRangeResponse
        {
            ExpensesTransaction = GetTransactionsRecords(transactions.Value, TransactionType.Expense),
            IncomesTransaction = GetTransactionsRecords(transactions.Value, TransactionType.Income)
        });
    }

    private static List<TransactionRecord>? GetTransactionsRecords(List<TransactionDataDto> transactions, TransactionType transactionType)
    {
        return new List<TransactionRecord>(transactions.Where(x => x.TransactionType == transactionType)
                        .Select(item => new TransactionRecord(
                         item.Date,
                         item.DayOfWeek,
                         item.Day,
                         item.TotalAmount,
                         item.TransactionDescriptionType,
                         item.Week,
                         item.Year,
                         item.Month
                        )));
    }
}