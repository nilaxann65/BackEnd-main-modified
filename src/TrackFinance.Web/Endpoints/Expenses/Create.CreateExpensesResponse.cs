namespace TrackFinance.Web.Endpoints.Expenses;
public class CreateExpenseResponse
{
    public int StatusResult { get; set; }
    public int ExpensesId { get; set; }

    public CreateExpenseResponse(int statusResult, int expensesId)
    {
        StatusResult = statusResult;
        ExpensesId = expensesId;
    }
}
