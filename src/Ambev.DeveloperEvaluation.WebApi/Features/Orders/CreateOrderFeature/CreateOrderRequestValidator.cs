using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Orders.CreateOrderFeature;

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.Items).NotEmpty();
        RuleForEach(x => x.Items).ChildRules(i =>
        {
            i.RuleFor(y => y.Description).NotEmpty().MaximumLength(200);
            i.RuleFor(y => y.UnitPrice).GreaterThan(0);
            i.RuleFor(y => y.Quantity).GreaterThan(0).LessThanOrEqualTo(20);
        });
    }
}