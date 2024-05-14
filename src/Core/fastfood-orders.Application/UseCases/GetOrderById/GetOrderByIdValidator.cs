using FluentValidation;

namespace fastfood_orders.Application.UseCases.GetOrderById;

public class GetOrderByIdValidator : AbstractValidator<GetOrderByIdRequest>
{
    public GetOrderByIdValidator() { }
}
