namespace TrackFinance.Web.Endpoints.Expenses;

public class UpdateExpenseResponse
{
    public ExpenseRecord _expenseRecord;

    public UpdateExpenseResponse(ExpenseRecord expenseRecord)
    {
        _expenseRecord = expenseRecord;
    }
}
