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
    [Description("Aucune")]
    None,
    [Description("Vegan")]
    Vegan,
    [Description("Végétarien")]
    Vegetarian,
    [Description("Sans gluten")]
    GlutenFree,
}

public enum ERecipeDifficulty
{
    [Description("Aucune")]
    None,
    [Description("Simple")]
    Easy,
    [Description("Normale")]
    Medium,
    [Description("Difficile")]
    Hard,
}

