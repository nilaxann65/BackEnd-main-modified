using TrackFinance.Core.TransactionAgregate.Enum;

namespace TrackFinance.Web.Endpoints.Incomes;

public class UpdateIncomeRequest
{
  public const string Route = "/Incomes";
  public int Id { get; set; }
  public string Description { get; set; } = string.Empty;
  public decimal Amount { get; set; }
  public TransactionDescriptionType ExpenseType { get; set; }
  public DateTime ExpenseDate { get; set; }
  public int UserId { get; set; }
}
