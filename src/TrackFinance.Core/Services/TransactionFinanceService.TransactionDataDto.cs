using TrackFinance.Core.TransactionAgregate.Enum;

namespace TrackFinance.Core.Services;

public class TransactionDataDto
{
  public DateTime Date { get; internal set; }
  public DayOfWeek? DayOfWeek { get; internal set; } = null;
  public int Day { get; internal set; }
  public decimal TotalAmount { get; internal set; }
  public TransactionDescriptionType TransactionDescriptionType { get; internal set; }
  public int Week { get; internal set; }
  public int Year { get; internal set; }
  public int Month { get; internal set; }
  public TransactionType TransactionType { get; internal set; }
}
