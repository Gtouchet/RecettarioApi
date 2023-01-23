using System.ComponentModel;

namespace RecettarioApi.Models.Database;

public class Recipe
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int CookingTimeInMinutes { get; set; }
    public string Difficulty { get; set; } = ERecipeDifficulty.None.ToString();
    public string Categories { get; set; } = null; // Separated by ::
    public string Steps { get; set; } = null; // Separated by ::
    public string ImageUrl { get; set; }

    public List<RecipeIngredient> Ingredients { get; set; }
}

public enum ERecipeCategory
{
    Aucune,
    Vegan,
    Vegetarian,
    GlutenFree,
}

public enum ERecipeDifficulty
{
    [Description("No lo sé")]
    None,
    [Description("Ez")]
    Easy,
    [Description("Isok")]
    Medium,
    [Description("Oof")]
    Hard,
}

