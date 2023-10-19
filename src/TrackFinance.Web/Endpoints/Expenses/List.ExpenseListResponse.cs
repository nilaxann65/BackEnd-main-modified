namespace TrackFinance.Web.Endpoints.Expenses;

public class ExpenseListResponse
{
    public List<ExpenseRecord> Expenses { get; set; } = new();
}