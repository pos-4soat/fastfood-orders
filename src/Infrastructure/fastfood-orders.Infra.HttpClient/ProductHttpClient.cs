using fastfood_orders.Domain.Contracts.Http;
using fastfood_orders.Domain.Models.Response;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;

namespace fastfood_orders.Infra.Http;

public class ProductHttpClient : HttpClient, IProductHttpClient
{
    public ProductHttpClient(string baseAddress)
    {
        BaseAddress = new Uri(baseAddress);
    }

    public async Task<ProductData> GetProductInfo(int productId, CancellationToken cancellationToken)
    {
        using HttpResponseMessage response = await GetAsync(productId.ToString(), cancellationToken);

        if (!response.IsSuccessStatusCode)
            return null;

        try
        {
            ProductResponse? responseObj = await response.Content.ReadFromJsonAsync<ProductResponse>(cancellationToken);

            return responseObj.body;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
