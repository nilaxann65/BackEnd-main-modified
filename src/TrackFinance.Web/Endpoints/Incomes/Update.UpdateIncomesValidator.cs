using FluentValidation;
using TrackFinance.Web.Endpoints.Incomes;

namespace TrackFinance.Web.Endpoints.Expense;

public class UpdateIncomesValidator : AbstractValidator<UpdateIncomeRequest>
{
  public UpdateIncomesValidator() 
  { 
    RuleFor(expense => expense.Amount).GreaterThan(0);
    RuleFor(expense => expense.Description).NotEmpty().NotNull();
  }
}
