using System.Globalization;
using Ardalis.Result;
using TrackFinance.Core.Interfaces;
using TrackFinance.Core.TransactionAgregate;
using TrackFinance.Core.TransactionAgregate.Enum;
using TrackFinance.Core.TransactionAgregate.Specifications;
using TrackFinance.SharedKernel.Interfaces;

namespace TrackFinance.Core.Services;

public class TransactionFinanceService : ITransactionFinanceService
{
  private readonly IRepository<Transaction> _repository;

  public TransactionFinanceService(IRepository<Transaction> repository)
  {
    _repository = repository;
  }

  public async Task<Result<List<TransactionDataDto>>> GetTransactionItemsByAsync(DateType dateType, int userId, TransactionType transactionType, CancellationToken cancellationToken = default)
  {
    switch (dateType)
    {
      case DateType.Day:
        var itemByDay = await _repository.ListAsync(new ItemsByDaySpec(userId, transactionType), cancellationToken);
        return GetListTransactionByDay(itemByDay);

      case DateType.Week:
        var itemByWeek = await _repository.ListAsync(new ItemsByWeekSpec(userId, transactionType), cancellationToken);
        return GetListTransactionByWeek(itemByWeek);

      case DateType.Month:
        var itemByMonth = await _repository.ListAsync(new ItemsByMonthSpec(userId, transactionType), cancellationToken);
        return GetListTransactionByMonth(itemByMonth);

      default: return Result<List<TransactionDataDto>>.NotFound();
    }
  }
  public async Task<Result<List<TransactionDataDto>>> GetTransactionItemsForLineChartsAsync(DateType dateType, int userId, TransactionType transactionType, CancellationToken cancellationToken = default)
  {
    switch (dateType)
    {
      case DateType.Day:
        var itemByDay = await _repository.ListAsync(new ItemsByDaySpec(userId, transactionType), cancellationToken);
        return GetListTransactionByDayForLineCharts(itemByDay);

      case DateType.Week:
        var itemByWeek = await _repository.ListAsync(new ItemsByWeekSpec(userId, transactionType), cancellationToken);
        return GetListTransactionByWeekForLineCharts(itemByWeek);

      case DateType.Month:
        var itemByMonth = await _repository.ListAsync(new ItemsByMonthSpec(userId, transactionType), cancellationToken);
        return GetListTransactionByMonthForLineCharts(itemByMonth);

      default: return Result<List<TransactionDataDto>>.NotFound();
    }
  }

  private static Result<List<TransactionDataDto>> GetListTransactionByMonth(List<Transaction> itemByMonth)
  {
    var list = new List<TransactionDataDto>();

    var items = itemByMonth.Select(h => new
    {
      h.ExpenseDate.Year,
      h.ExpenseDate.Month,
      h.Amount,
      h.TransactionType,
      h.TransactionDescriptionType,
    })
    .GroupBy(h => new { h.Year, h.Month, h.TransactionDescriptionType, h.TransactionType })
    .Select(g => new
    {
      g.Key.Year,
      g.Key.Month,
      TotalAmount = g.Sum(h => h.Amount),
      g.Key.TransactionDescriptionType,
      g.Key.TransactionType
    })
    .OrderBy(g => g.Year)
    .ThenBy(g => g.Month)
    .ToList();

    foreach (var t in items)
    {
      var transactionItem = new TransactionDataDto
      {
        TotalAmount = t.TotalAmount,
        Month = t.Month,
        Year = t.Year,
        TransactionDescriptionType = t.TransactionDescriptionType,
        TransactionType = t.TransactionType
      };
      list.Add(transactionItem);
    }
    if (list.Count == 0) Result<List<TransactionDataDto>>.NotFound();
    return list;
  }

  private static Result<List<TransactionDataDto>> GetListTransactionByWeek(List<Transaction> itemByWeek)
  {
    var transactionsResult = new List<TransactionDataDto>();

    var transactions = itemByWeek.Select(h => new
    {
      h.ExpenseDate.Year,
      WeekNumber = GetWeekOfYear(h.ExpenseDate),
      h.Amount,
      h.TransactionDescriptionType,
      h.TransactionType
    }).GroupBy(h => new { h.TransactionType, h.Year, h.WeekNumber, h.TransactionDescriptionType })
          .Select(g => new
          {
            g.Key.Year,
            g.Key.WeekNumber,
            TotalAmount = g.Sum(h => h.Amount),
            g.Key.TransactionDescriptionType,
            g.Key.TransactionType
          })
          .OrderBy(g => g.Year)
          .ThenBy(g => g.WeekNumber).ToList();

    foreach (var transaction in transactions)
    {
      var transactionItem = new TransactionDataDto
      {
        TotalAmount = transaction.TotalAmount,
        Week = transaction.WeekNumber,
        Year = transaction.Year,
        TransactionDescriptionType = transaction.TransactionDescriptionType,
        TransactionType = transaction.TransactionType
      };
      transactionsResult.Add(transactionItem);
    }

    if (transactionsResult.Count == 0) Result<List<TransactionDataDto>>.NotFound();
    return transactionsResult;
  }

  private static Result<List<TransactionDataDto>> GetListTransactionByDay(List<Transaction> itemByDay)
  {
    var transactions = new List<TransactionDataDto>();
    var selectedValues = itemByDay.Select(t => new
    {
      t.ExpenseDate.Date,
      t.Amount,
      t.ExpenseDate.DayOfWeek,
      t.TransactionDescriptionType,
      t.TransactionType
    })
          .GroupBy(t => new { t.TransactionType, t.Date, t.DayOfWeek, t.TransactionDescriptionType })
          .Select(y => new { Date = y.Key, TotalAmount = y.Sum(x => x.Amount), y.Key.TransactionType })
          .OrderBy(x => x.Date.Date)
          .ToList();

    foreach (var t in selectedValues)
    {
      var transactionItem = new TransactionDataDto
      {
        Date = t.Date.Date,
        DayOfWeek = t.Date.DayOfWeek,
        Day = t.Date.Date.Day,
        TotalAmount = t.TotalAmount,
        TransactionDescriptionType = t.Date.TransactionDescriptionType,
        TransactionType = t.TransactionType
      };

      transactions.Add(transactionItem);
    }
    if (transactions.Count == 0) Result<List<TransactionDataDto>>.NotFound();

    return transactions;
  }
  private static Result<List<TransactionDataDto>> GetListTransactionByDayForLineCharts(List<Transaction> itemByDay)
  {
    var transactions = new List<TransactionDataDto>();
    var selectedValues = itemByDay.Select(t => new
    {
      t.ExpenseDate.Date,
      t.Amount,
      t.ExpenseDate.DayOfWeek,
      t.TransactionDescriptionType,
      t.TransactionType
    })
          .GroupBy(t => new { t.TransactionType, t.Date, t.DayOfWeek })
          .Select(y => new { Date = y.Key, TotalAmount = y.Sum(x => x.Amount), y.Key.TransactionType })
          .OrderBy(x => x.Date.Date)
          .ToList();

    foreach (var t in selectedValues)
    {
      var transactionItem = new TransactionDataDto
      {
        Date = t.Date.Date,
        DayOfWeek = t.Date.DayOfWeek,
        Day = t.Date.Date.Day,
        TotalAmount = t.TotalAmount,
        TransactionType = t.TransactionType
      };

      transactions.Add(transactionItem);
    }
    if (transactions.Count == 0) Result<List<TransactionDataDto>>.NotFound();

    return transactions;
  }
  private static Result<List<TransactionDataDto>> GetListTransactionByWeekForLineCharts(List<Transaction> itemByWeek)
  {
    var transactionsResult = new List<TransactionDataDto>();

    var transactions = itemByWeek.Select(h => new
    {
      h.ExpenseDate.Year,
      WeekNumber = GetWeekOfYear(h.ExpenseDate),
      h.Amount,
      h.TransactionDescriptionType,
      h.TransactionType
    }).GroupBy(h => new { h.TransactionType, h.Year, h.WeekNumber })
          .Select(g => new
          {
            g.Key.Year,
            g.Key.WeekNumber,
            TotalAmount = g.Sum(h => h.Amount),
            g.Key.TransactionType
          })
          .OrderBy(g => g.Year)
          .ThenBy(g => g.WeekNumber).ToList();

    foreach (var transaction in transactions)
    {
      var transactionItem = new TransactionDataDto
      {
        TotalAmount = transaction.TotalAmount,
        Week = transaction.WeekNumber,
        Year = transaction.Year,
        TransactionType = transaction.TransactionType
      };
      transactionsResult.Add(transactionItem);
    }

    if (transactionsResult.Count == 0) Result<List<TransactionDataDto>>.NotFound();
    return transactionsResult;
  }
  private static Result<List<TransactionDataDto>> GetListTransactionByMonthForLineCharts(List<Transaction> itemByMonth)
  {
    var list = new List<TransactionDataDto>();

    var items = itemByMonth.Select(h => new
    {
      h.ExpenseDate.Year,
      h.ExpenseDate.Month,
      h.Amount,
      h.TransactionType,
      h.TransactionDescriptionType,
    })
    .GroupBy(h => new { h.Year, h.Month, h.TransactionType })
    .Select(g => new
    {
      g.Key.Year,
      g.Key.Month,
      TotalAmount = g.Sum(h => h.Amount),
      g.Key.TransactionType
    })
    .OrderBy(g => g.Year)
    .ThenBy(g => g.Month)
    .ToList();

    foreach (var t in items)
    {
      var transactionItem = new TransactionDataDto
      {
        TotalAmount = t.TotalAmount,
        Month = t.Month,
        Year = t.Year,
        TransactionType = t.TransactionType
      };
      list.Add(transactionItem);
    }
    if (list.Count == 0) Result<List<TransactionDataDto>>.NotFound();
    return list;
  }

  private static int GetWeekOfYear(DateTime time)
  {
    var cal = CultureInfo.InvariantCulture.Calendar;
    var day = (int)cal.GetDayOfWeek(time);

    if (day is >= (int)DayOfWeek.Monday and <= (int)DayOfWeek.Wednesday)
    {
      time = time.AddDays(3);
    }
    return cal.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
  }
}
