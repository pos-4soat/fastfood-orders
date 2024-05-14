using FluentValidation;

namespace fastfood_orders.Application.UseCases.GetAllOrders;

public class GetAllOrdersValidators : AbstractValidator<GetAllOrdersRequest>
{
    public GetAllOrdersValidators()
    {
    }
}
