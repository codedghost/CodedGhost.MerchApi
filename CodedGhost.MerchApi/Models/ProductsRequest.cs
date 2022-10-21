using Newtonsoft.Json;

namespace CodedGhost.MerchApi.Models;

public class ProductsRequest
{
    [JsonProperty("CategoryIds")]
    public int[] CategoryIds { get; set; }
}