using AutoMapper;
using fastfood_orders.Application.Shared.BaseResponse;
using fastfood_orders.Domain.Contracts.Repository;
using fastfood_orders.Domain.Entity;
using MediatR;

namespace fastfood_orders.Application.UseCases.UpdateOrder;

public class UpdateOrderHandler : IRequestHandler<UpdateOrderRequest, Result<UpdateOrderResponse>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public UpdateOrderHandler(
        IMapper mapper,
        IOrderRepository orderRepository
        )
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    }

    public async Task<Result<UpdateOrderResponse>> Handle(UpdateOrderRequest request, CancellationToken cancellationToken)
    {
        OrderEntity orderEntity = _mapper.Map<OrderEntity>(request);

        OrderEntity? existentOrder = await _orderRepository.GetOrderAsync(orderEntity.Id, cancellationToken);

        if (existentOrder == null)
            return Result<UpdateOrderResponse>.Failure("OBE010");

        if (!existentOrder.ValidStatus(orderEntity.Status))
            return Result<UpdateOrderResponse>.Failure("OBE014");

        existentOrder.Status = orderEntity.Status;
        await _orderRepository.EditOrderAsync(existentOrder, cancellationToken);

        return Result<UpdateOrderResponse>.Success(_mapper.Map<UpdateOrderResponse>(existentOrder));
    }
}
