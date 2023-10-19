using FluentValidation;

namespace TrackFinance.Web.Endpoints.Expenses;

public class GetByIdExpensesValidator : AbstractValidator<GetByIdExpenseRequest>
{
    public GetByIdExpensesValidator()
    {
        RuleFor(expense => expense.ExpenseId).GreaterThan(0);
    }
}