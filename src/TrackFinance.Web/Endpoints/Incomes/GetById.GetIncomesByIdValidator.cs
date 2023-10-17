using FluentValidation;
using TrackFinance.Web.Endpoints.Incomes;

namespace TrackFinance.Web.Endpoints.Expense;

public class GetIncomesByIdValidator : AbstractValidator<GetIncomeByIdRequest>
{
  public GetIncomesByIdValidator()
  {
    RuleFor(expense => expense.IncomeId).GreaterThan(0);
  }
}
