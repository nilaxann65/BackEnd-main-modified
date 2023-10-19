namespace TrackFinance.Web.Endpoints.Balance;

public class GetListByDateRangeResponse
{
    public List<TransactionRecord>? ExpensesTransaction { get; set; }
    public List<TransactionRecord>? IncomesTransaction { get; set; }
}