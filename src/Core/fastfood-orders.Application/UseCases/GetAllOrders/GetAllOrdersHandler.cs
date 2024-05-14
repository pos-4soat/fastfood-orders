using AutoMapper;
using fastfood_orders.Application.Shared.BaseResponse;
using fastfood_orders.Domain.Contracts.Http;
using fastfood_orders.Domain.Contracts.Repository;
using fastfood_orders.Domain.Models.Response;
using MediatR;

namespace fastfood_orders.Application.UseCases.GetAllOrders;

public class GetAllOrdersHandler(
    IMapper mapper,
    IOrderRepository orderRepository,
    IProductHttpClient httpClient) : IRequestHandler<GetAllOrdersRequest, Result<GetAllOrdersResponse>>
{
    private readonly IOrderRepository _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    private readonly IProductHttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

    public async Task<Result<GetAllOrdersResponse>> Handle(GetAllOrdersRequest request, CancellationToken cancellationToken)
    {
        List<Domain.Entity.OrderEntity> result = (await _orderRepository.GetAllAsync(cancellationToken)).ToList();

        IEnumerable<Task<GetAllOrdersOrder>> orders = result.Select(async orderEntity =>
        {
            GetAllOrdersOrder order = _mapper.Map<GetAllOrdersOrder>(orderEntity);

            order.Items = await Task.WhenAll(orderEntity.OrderedItems.Select(async item =>
            {
                ProductData product = await _httpClient.GetProductInfo(item.ProductId, cancellationToken);
                return new GetAllOrdersProductData
                {
                    ProductId = item.ProductId,
                    Price = product.Price,
                    Name = product.Name,
                    Quantity = item.Quantity,
                };
            }));

            return order;
        }).OrderBy(x => x.Status);

        List<GetAllOrdersOrder> ordersList = (await Task.WhenAll(orders)).ToList();

        GetAllOrdersResponse response = new GetAllOrdersResponse(ordersList);

        return Result<GetAllOrdersResponse>.Success(response);
    }
}
