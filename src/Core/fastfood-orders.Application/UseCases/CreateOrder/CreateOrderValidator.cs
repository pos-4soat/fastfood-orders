using FluentValidation;

namespace fastfood_orders.Application.UseCases.CreateOrder;

public class CreateOrderValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderValidator()
    {
        RuleFor(dto => dto.OrderedItems)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("OBE005")
            .ForEach(item => item.SetValidator(new OrderItensValidator()));
    }
}

public class OrderItensValidator : AbstractValidator<OrderItens>
{
    public OrderItensValidator()
    {
        RuleFor(dto => dto.ProductId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("OBE006");

        RuleFor(dto => dto.Quantity)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithMessage("OBE007");
    }
}