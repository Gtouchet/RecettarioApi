namespace RecettarioApi.Models.Database;

public class RecipeIngredient
{
    public Guid Id { get; set; }
    public Article Article { get; set; }
    public int Quantity { get; set; }
}
