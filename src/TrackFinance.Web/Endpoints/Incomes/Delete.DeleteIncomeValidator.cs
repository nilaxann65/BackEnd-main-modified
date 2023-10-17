using FluentValidation;
using TrackFinance.Web.Endpoints.Incomes;

namespace TrackFinance.Web.Endpoints.Expense;

public class DeleteIncomeValidator : AbstractValidator<DeleteIncomeRequest>
{
  public DeleteIncomeValidator()
  {
    RuleFor(expense => expense.IncomeId).GreaterThan(0);
  }
}
