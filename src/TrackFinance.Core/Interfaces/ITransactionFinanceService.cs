using Ardalis.Result;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TrackFinance.Core.Services;
using TrackFinance.Core.TransactionAgregate;
using TrackFinance.Core.TransactionAgregate.Enum;

namespace TrackFinance.Core.Interfaces;
public interface ITransactionFinanceService
{
  public Task<Result<List<TransactionDataDto>>> GetTransactionItemsByAsync(DateType dateType, int userId, TransactionType transactionType, CancellationToken cancellationToken = default);
  public Task<Result<List<TransactionDataDto>>> GetTransactionItemsForLineChartsAsync(DateType dateType, int userId, TransactionType transactionType, CancellationToken cancellationToken = default);
  public Task<Result<List<TransactionDataDto>>> GetTransactionItemsByDateRangeAsync(DateTime startDate, DateTime endDate, int userId, TransactionType transactionType, CancellationToken cancellationToken = default);
}

