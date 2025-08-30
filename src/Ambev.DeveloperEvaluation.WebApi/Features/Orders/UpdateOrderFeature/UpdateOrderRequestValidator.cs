using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Orders.UpdateOrderFeature;

public class UpdateOrderRequestValidator : AbstractValidator<UpdateOrderRequest>
{
    public UpdateOrderRequestValidator()
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