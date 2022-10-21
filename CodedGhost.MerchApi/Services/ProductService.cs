using CodedGhost.MerchApi.Interfaces.Services;
using PrintfulLib.Interfaces.ExternalClients;
using PrintfulLib.Models.ApiRequest.Product;
using PrintfulLib.Models.ChildObjects;

namespace CodedGhost.MerchApi.Services;

public class ProductService : IProductService
{
    private readonly IPrintfulClient _printfulClient;

    public ProductService(IPrintfulClient printfulClient)
    {
        _printfulClient = printfulClient;
    }

    public async Task<SyncProduct[]> GetProducts(int[] filterCategoryIds)
    {
        var productResponse = await _printfulClient.GetProducts(new GetProductsRequest
        {
            CategoryIds = filterCategoryIds
        }).ConfigureAwait(false);

        return productResponse.Result;
    }
}