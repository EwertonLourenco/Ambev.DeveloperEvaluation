using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Orders.CreateOrder;

public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.Items).NotEmpty();

        RuleForEach(x => x.Items).ChildRules(i =>
        {
            i.RuleFor(y => y.Description).NotEmpty().MaximumLength(200);
            i.RuleFor(y => y.UnitPrice).GreaterThan(0);
            i.RuleFor(y => y.Quantity).GreaterThan(0).LessThanOrEqualTo(20); // regra dos 20 por item já na borda
        });
    }
}