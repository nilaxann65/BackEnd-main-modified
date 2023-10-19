using FluentValidation;

namespace TrackFinance.Web.Endpoints.Expenses;

public class CreateExpensesValidator : AbstractValidator<CreateExpenseRequest>
{
    public CreateExpensesValidator()
    {
        RuleFor(expense => expense.Description).NotEmpty().NotNull();
        RuleFor(expense => expense.Amount).GreaterThan(0);
    }
}
