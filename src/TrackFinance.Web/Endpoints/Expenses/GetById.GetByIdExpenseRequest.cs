namespace TrackFinance.Web.Endpoints.Expenses;

public class GetByIdExpenseRequest
{
    public const string Route = "Expenses/{ExpenseId:int}";
    public static string BuildRoute(int expenseId) => Route.Replace("{ExpenseId:int}", expenseId.ToString());
    public int ExpenseId { get; set; }
}