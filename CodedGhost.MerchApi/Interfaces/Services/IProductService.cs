using PrintfulLib.Models.ChildObjects;

namespace CodedGhost.MerchApi.Interfaces.Services;

public interface IProductService
{
    Task<SyncProduct[]> GetProducts(int[] filterCategoryIds);

    Task<SyncProductInfo> GetProduct(int productId);
}