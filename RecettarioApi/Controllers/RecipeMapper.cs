using RecettarioApi.Models.Database;
using RecettarioApi.Models.Response;

namespace RecettarioApi.Controllers;

public class RecipeMapper
{
    public RecipeResponse RecipeToResponse(Recipe recipe) => new()
    {
        Id = recipe.Id,
        Name = recipe.Name,
        CookingTimeInMinutes = recipe.CookingTimeInMinutes,
        Difficulty = recipe.Difficulty,
        Categories = Utils.ParseRecipeCategoriesToList(recipe.Categories),
        Steps = Utils.ParseRecipeStepsToList(recipe.Steps),
        ImageUrl = recipe.ImageUrl,
        Ingredients = recipe.Ingredients,
    };
}
