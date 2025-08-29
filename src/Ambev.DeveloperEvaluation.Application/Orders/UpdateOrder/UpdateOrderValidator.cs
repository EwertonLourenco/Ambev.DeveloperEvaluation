using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Orders.UpdateOrder;

public class UpdateOrderValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Items).NotEmpty();
        RuleForEach(x => x.Items).ChildRules(i =>
        {
            i.RuleFor(y => y.Description).NotEmpty().MaximumLength(200);
            i.RuleFor(y => y.UnitPrice).GreaterThan(0);
            i.RuleFor(y => y.Quantity).GreaterThan(0).LessThanOrEqualTo(20);
        });
    }
}