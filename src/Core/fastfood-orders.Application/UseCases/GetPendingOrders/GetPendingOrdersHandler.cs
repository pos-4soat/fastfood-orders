using AutoMapper;
using fastfood_orders.Application.Shared.BaseResponse;
using fastfood_orders.Domain.Contracts.Http;
using fastfood_orders.Domain.Contracts.Repository;
using fastfood_orders.Domain.Models.Response;
using MediatR;

namespace fastfood_orders.Application.UseCases.GetPendingOrders;

public class GetPendingOrdersHandler : IRequestHandler<GetPendingOrdersRequest, Result<GetPendingOrdersResponse>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly IProductHttpClient _httpClient;

    public GetPendingOrdersHandler(
        IMapper mapper,
        IOrderRepository orderRepository,
        IProductHttpClient httpClient)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<Result<GetPendingOrdersResponse>> Handle(GetPendingOrdersRequest request, CancellationToken cancellationToken)
    {
        List<Domain.Entity.OrderEntity> pendingOrders = (await _orderRepository.GetPendingOrders(cancellationToken)).ToList();

        List<GetPendingOrdersOrder> orders = [];

        pendingOrders.ForEach(orderEntity =>
        {
            GetPendingOrdersOrder order = _mapper.Map<GetPendingOrdersOrder>(orderEntity);

            List<GetPendingOrdersProductData> updatedItems = order.Items.Select(async item =>
            {
                ProductData product = await _httpClient.GetProductInfo(item.ProductId, cancellationToken);
                return new { item, product };
            })
            .Select(t => new GetPendingOrdersProductData
            {
                ProductId = t.Result.item.ProductId,
                Price = t.Result.item.Price,
                Name = t.Result.item.Name,
                Quantity = t.Result.item.Quantity,
            })
            .ToList();

            order.Items = updatedItems;

            orders.Add(order);
        });


        GetPendingOrdersResponse response = new GetPendingOrdersResponse(orders);

        return Result<GetPendingOrdersResponse>.Success(response);
    }
}
