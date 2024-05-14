using FluentValidation;

namespace fastfood_orders.Application.UseCases.UpdateOrder;

public class UpdateOrderValidator : AbstractValidator<UpdateOrderRequest>
{
    public UpdateOrderValidator()
    {
        RuleFor(dto => dto.Id)
            .GreaterThan(0)
            .WithMessage("OBE012");

        RuleFor(dto => dto.Status)
            .IsInEnum()
            .WithMessage("OBE013");
    }
}
