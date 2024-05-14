using fastfood_orders.Domain.Models.Response;

namespace fastfood_orders.Domain.Contracts.Http
{
    public interface IProductHttpClient
    {
        Task<ProductData> GetProductInfo(int productId, CancellationToken cancellationToken);
    }
}
