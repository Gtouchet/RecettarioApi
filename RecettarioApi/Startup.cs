using RecettarioApi.Models;
using RecettarioApi.Models.Database;
using System.Text.Json;

namespace RecettarioApi;

public abstract class Startup
{
    public async static Task InitializeDatabase(Context context)
    {
        await Startup.CreateIngredients(context);
        await Startup.CreateRecipes(context);
    }

    public static async Task CreateIngredients(Context context)
    {
        JsonSerializer.DeserializeAsync<List<Article>>(File.OpenRead("Data/articles.json")).Result.ForEach(a => context.Articles.Add(a));
        await context.SaveChangesAsync();
    }

    public static async Task CreateRecipes(Context context)
    {
        JsonSerializer.DeserializeAsync<List<JsonRecipe>>(File.OpenRead("Data/recipes.json")).Result.ForEach(r =>
        {
            context.Recipes.Add(new()
            {
                Id = r.Id,
                Name = r.Name,
                CookingTimeInMinutes = r.CookingTimeInMinutes > 0 ? r.CookingTimeInMinutes : 0,
                Difficulty = Utils.ParseStringAs<ERecipeDifficulty>(r.Difficulty).ToString(),
                Categories = r.Categories == null ? null : r.Categories
                    .Split("::")
                    .ToList()
                    .Select(c => Utils.ParseStringAs<ERecipeCategory>(c).ToString())
                    .Aggregate((a, b) => a + "::" + b),
                Steps = r.Steps,
                ImageUrl = r.ImageUrl,
                Ingredients = r.IngredientQuantities
                    .ToList()
                    .Select(ingredientQuantities => new RecipeIngredient()
                    {
                        Article = context.Articles.ToList().FirstOrDefault(a => a.Name.Equals(ingredientQuantities.Key, StringComparison.OrdinalIgnoreCase)) ??
                            new Article() { Id = Guid.Parse("00000000-0000-0000-0000-000000000000"), Name = "Article inconnu", },
                        Quantity = ingredientQuantities.Value,
                    })
                    .ToList()
            });
        });
        await context.SaveChangesAsync();
    }

    internal class JsonRecipe : Recipe
    {
        public Dictionary<string, int> IngredientQuantities { get; set; }
    }
}
