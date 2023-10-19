using Ardalis.Specification;
using TrackFinance.Core.TransactionAgregate.Enum;

namespace TrackFinance.Core.TransactionAgregate.Specifications;

public class ItemsByDateRangeSpec : Specification<Transaction>, ISingleResultSpecification
{
    public ItemsByDateRangeSpec(DateTime startDate, DateTime endDate, int userId, TransactionType transactionType)
    {
        Query.Where(h => h.ExpenseDate.Date >= startDate.Date && h.ExpenseDate.Date <= endDate.Date)
             .Where(h => (transactionType == TransactionType.All) || h.TransactionType == transactionType)
             .Where(h => h.UserId == userId)
             .OrderBy(g => g.ExpenseDate);
    }
}