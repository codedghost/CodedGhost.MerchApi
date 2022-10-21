using CodedGhost.MerchApi.Interfaces.Services;
using CodedGhost.MerchApi.Models;
using PrintfulLib.Interfaces.ExternalClients;
using PrintfulLib.Models.ChildObjects;

namespace CodedGhost.MerchApi.Services;

public class CategoryService : ICategoryService
{
    private readonly IPrintfulClient _printfulClient;

    public CategoryService(IPrintfulClient printfulClient)
    {
        _printfulClient = printfulClient;
    }

    public async Task<CategoryNode[]> GetCategories()
    {
        var categories = await _printfulClient.GetCategories().ConfigureAwait(false);

        var categoryNodes = FormatCategories(categories.CategoriesContainer.Categories);

        return categoryNodes;
    }

    private CategoryNode[] FormatCategories(Category[] categoriesContainerCategories)
    {
        var groups = categoriesContainerCategories.GroupBy(i => i.ParentId).ToList();

        var rootsGrouping = groups.FirstOrDefault(g => g.Key == 0);

        if (rootsGrouping == null) return new CategoryNode[] { };

        var roots = GetNodes(rootsGrouping.ToList());

        if (roots.Length > 0)
        {
            var dict = groups.Where(g => g.Key != 0).ToDictionary(g => g.Key, g => g.ToList());

            foreach (var t in roots)
                AddChildren(t, dict);
        }

        return roots;
    }

    private void AddChildren(CategoryNode node, Dictionary<int, List<Category>> dict)
    {
        if (dict.ContainsKey(node.Id))
        {
            node.ChildElements = GetNodes(dict[node.Id].ToList());
            foreach (var t in node.ChildElements)
                AddChildren(t, dict);
        }
        else
        {
            node.ChildElements = new CategoryNode[] { };
        }
    }

    private static CategoryNode[] GetNodes(List<Category> categories)
    {
        return categories.Select(r => new CategoryNode
        {
            Id = r.Id,
            ParentId = r.ParentId,
            ImageUrl = r.ImageUrl,
            Size = r.Size,
            Title = r.Title,
            ChildElements = new CategoryNode[] { }
        }).ToArray();
    }
}