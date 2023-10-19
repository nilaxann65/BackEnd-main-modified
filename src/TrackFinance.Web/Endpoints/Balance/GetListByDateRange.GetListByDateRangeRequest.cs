using TrackFinance.Core.TransactionAgregate;
using TrackFinance.Core.TransactionAgregate.Enum;

namespace TrackFinance.Web.Endpoints.Balance;
public class GetListByDateRangeRequest
{
    public const string Route = "/Balance/DateRange/{UserId}/{TransactionType}/{StartDate}/{EndDate}";
    public int UserId { get; set; }
    public TransactionType TransactionType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
