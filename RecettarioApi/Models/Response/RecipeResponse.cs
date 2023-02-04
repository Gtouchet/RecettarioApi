namespace RecettarioApi.Models.Response;

public class RecipeResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int CookingTimeInMinutes { get; set; }
    public string Difficulty { get; set; }
    public List<string> Categories { get; set; }
    public List<string> Steps { get; set; }
    public string ImageUrl { get; set; }

    public List<ShoppingItem> Ingredients { get; set; }
}
