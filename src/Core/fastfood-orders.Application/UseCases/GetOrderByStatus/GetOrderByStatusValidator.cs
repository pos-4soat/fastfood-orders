using FluentValidation;

namespace fastfood_orders.Application.UseCases.GetOrderByStatus
{
    public class GetOrderByStatusValidator : AbstractValidator<GetOrderByStatusRequest>
    {
        public GetOrderByStatusValidator()
        {
            RuleFor(dto => dto.Status)
                .IsInEnum()
                .WithMessage("OBE011");
        }
    }
}
