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
        var productsResponse = await _printfulClient.GetProducts(new GetProductsRequest
        {
            CategoryIds = filterCategoryIds
        }).ConfigureAwait(false);

        return productsResponse.Result;
    }

    public async Task<SyncProductInfo> GetProduct(int productId)
    {
        var productResponse = await _printfulClient.GetProductAndVariants(new GetProductAndVariantsRequest
        {
            ProductId = productId
        }).ConfigureAwait(false);

        return productResponse.Result;
    }
}