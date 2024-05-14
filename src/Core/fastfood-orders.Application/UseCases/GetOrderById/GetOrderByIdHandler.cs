using AutoMapper;
using fastfood_orders.Application.Shared.BaseResponse;
using fastfood_orders.Domain.Contracts.Http;
using fastfood_orders.Domain.Contracts.Repository;
using fastfood_orders.Domain.Models.Response;
using MediatR;

namespace fastfood_orders.Application.UseCases.GetOrderById;

public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdRequest, Result<GetOrderByIdResponse>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly IProductHttpClient _httpClient;

    public GetOrderByIdHandler(IMapper mapper, IOrderRepository orderRepository, IProductHttpClient httpClient)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<Result<GetOrderByIdResponse>> Handle(GetOrderByIdRequest request, CancellationToken cancellationToken)
    {
        Domain.Entity.OrderEntity? result = await _orderRepository.GetOrderAsync(request.OrderId, cancellationToken);

        if (result == null)
            return Result<GetOrderByIdResponse>.Failure("OBE010");

        GetOrderByIdResponse response = _mapper.Map<GetOrderByIdResponse>(result);

        response.Items = await Task.WhenAll(response.Items.Select(async item =>
        {
            ProductData product = await _httpClient.GetProductInfo(item.ProductId, cancellationToken);
            return new GetOrderByIdProductData
            {
                ProductId = item.ProductId,
                Price = product.Price,
                Name = product.Name,
                Quantity = item.Quantity,
            };
        }));

        return Result<GetOrderByIdResponse>.Success(response);
    }

}
