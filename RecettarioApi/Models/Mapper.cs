using RecettarioApi.Models.Database;
using RecettarioApi.Models.Response;

namespace RecettarioApi.Models;

public class Mapper
{
    public RecipeResponse RecipeToResponse(Recipe recipe)
    {
        return new RecipeResponse()
        {
            Id = recipe.Id,
            Name = recipe.Name,
            CookingTimeInMinutes = recipe.CookingTimeInMinutes,
            Difficulty = Utils.GetEnumDescription(Utils.ParseStringAs<ERecipeDifficulty>(recipe.Difficulty)),
            Categories = Utils.ParseRecipeCategoriesToList(recipe.Categories),
            Steps = Utils.ParseRecipeStepsToList(recipe.Steps),
            ImageUrl = recipe.ImageUrl,
            Ingredients = recipe.Ingredients
                .Select(i => this.RecipeIngredientToShoppingItem(i))
                .ToList(),
        };
    }

    public ArticleResponse ArticleToResponse(Article article)
    {
        return new ArticleResponse()
        {
            Id = article.Id,
            Name = article.Name,
            Category = Utils.GetEnumDescription(Utils.ParseStringAs<EArticleCategory>(article.Category)),
            ImageUrl = article.ImageUrl,
        };
    }

    public ShoppingItem RecipeIngredientToShoppingItem(RecipeIngredient ingredient)
    {
        return new ShoppingItem()
        {
            Id = ingredient.Article.Id,
            Name = ingredient.Article.Name,
            Category = Utils.GetEnumDescription(Utils.ParseStringAs<EArticleCategory>(ingredient.Article.Category)),
            UnconvertedQuantity = ingredient.Quantity,
            Unit = Utils.ParseStringAs<EQuantityType>(ingredient.Article.Unit),
            ImageUrl = ingredient.Article.ImageUrl,
        };
    }
}
