using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using RecettarioApi.Models;
using RecettarioApi.Models.Database;
using System.Text.Json;

namespace RecettarioApi;

public abstract class Startup
{
    private static readonly string azureContainerConnectionString =
        "DefaultEndpointsProtocol=https;" +
        "AccountName=recettariostorage;" +
        "AccountKey=nl1jfW46iYVB4nNz6K8EzRabUWNhwe6b4TcWUIiwxmN95aXp8xHedK47zznNHkwfNBSE1BjsJQH++AStitvRxg==;" +
        "EndpointSuffix=core.windows.net";

    public async static Task InitializeDatabase(Context context)
    {
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Startup.azureContainerConnectionString);
        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
        CloudBlobContainer container = blobClient.GetContainerReference("data");

        await Startup.ReadIngredients(container, context);
        await Startup.ReadRecipes(container, context);
    }

    private static async Task ReadIngredients(CloudBlobContainer container, Context context)
    {
        CloudBlockBlob articlesFile = container.GetBlockBlobReference("articles.json");
        string articlesJson = await articlesFile.DownloadTextAsync();
        JsonSerializer.Deserialize<List<Article>>(articlesJson).ForEach(a => context.Articles.Add(a));
        await context.SaveChangesAsync();
    }

    private static async Task ReadRecipes(CloudBlobContainer container, Context context)
    {
        CloudBlockBlob recipesFile = container.GetBlockBlobReference("recipes.json");
        string recipesJson = await recipesFile.DownloadTextAsync();
        JsonSerializer.Deserialize<List<JsonRecipe>>(recipesJson).ForEach(r =>
        {
            context.Recipes.Add(new Recipe()
            {
                Id = r.Id,
                Name = r.Name,
                CookingTimeInMinutes = r.CookingTimeInMinutes > 0 ? r.CookingTimeInMinutes : 0,
                // Set difficulty as default if not valid
                Difficulty = Utils.ParseStringAs<ERecipeDifficulty>(r.Difficulty).ToString(),
                Categories = r.Categories == null ? null : r.Categories
                    .Split("::")
                    .ToList()
                    // Set category as default if not valid
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
