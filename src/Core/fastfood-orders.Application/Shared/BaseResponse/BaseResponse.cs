using fastfood_orders.Domain.Enum;
using Newtonsoft.Json;

namespace fastfood_orders.Application.Shared.BaseResponse;

public class BaseResponse(StatusResponse status)
{
    /// <summary>
    /// Versão da api.
    /// </summary>
    /// <example>1.0.0</example>
    [JsonProperty(Order = 1)]
    public string Version { get => "1.0.0"; }

    /// <summary>
    /// Status.
    /// </summary>
    /// <example>SUCCESS/ERROR</example>
    [JsonProperty(Order = 2)]
    public string Status { get; set; } = status.ToString();
}