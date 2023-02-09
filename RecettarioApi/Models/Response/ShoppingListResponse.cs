using RecettarioApi.Models.Database;
using System.Text.Json.Serialization;

namespace RecettarioApi.Models.Response;

public class ShoppingListResponse
{
    public List<ShoppingItem> ShoppingList { get; set; }
    public string Warning { get; set; }
}

public class ShoppingItem
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public string ImageUrl { get; set; }
    public float Quantity { get; set; }
    public string Unit { get; set; }
}
