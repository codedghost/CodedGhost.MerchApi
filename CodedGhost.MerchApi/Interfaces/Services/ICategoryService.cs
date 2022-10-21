using CodedGhost.MerchApi.Models;

namespace CodedGhost.MerchApi.Interfaces.Services;

public interface ICategoryService
{
    Task<CategoryNode[]> GetCategories();
}