using FluentValidation;

namespace fastfood_orders.Application.UseCases.GetPendingOrders;

public class GetPendingOrdersValidator : AbstractValidator<GetPendingOrdersRequest>
{
    public GetPendingOrdersValidator()
    {

    }
}
