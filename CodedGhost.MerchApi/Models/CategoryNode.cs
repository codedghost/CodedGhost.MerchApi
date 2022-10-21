using Newtonsoft.Json;
using PrintfulLib.Models.ChildObjects;

namespace CodedGhost.MerchApi.Models;

public class CategoryNode : Category
{
    [JsonProperty("children")]
    public CategoryNode[] ChildElements { get; set; }
}