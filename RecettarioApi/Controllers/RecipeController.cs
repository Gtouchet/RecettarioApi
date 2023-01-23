using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecettarioApi.Models;
using RecettarioApi.Models.Database;
using RecettarioApi.Models.Response;

namespace RecettarioApi.Controllers;

#pragma warning disable CS1998

[ApiController]
[Route("[controller]")]
public class RecipeController : ControllerBase
{
    private readonly Context context;
    private readonly RecipeMapper recipeMapper;

    public RecipeController(
        Context context,
        RecipeMapper recipeMapper)
    {
        this.context = context;
        this.recipeMapper = recipeMapper;
    }
    
    [HttpGet]
    public async Task<ActionResult> Read(Guid? id = null)
    {
        IQueryable<Recipe> query = this.context.Recipes
            .Include(r => r.Ingredients)
                .ThenInclude(i => i.Article);

        if (id != null)
        {
            query = query.Where(r => r.Id.Equals(id));
        }
        
        List<RecipeResponse> recipes = await query
            .Select(r => this.recipeMapper.RecipeToResponse(r))
            .ToListAsync();
        
        return id == null ?
            Ok(recipes) : recipes.Count > 0 ?
                Ok(recipes.First()) : NotFound("La recette n'existe pas");
    }

    [HttpGet("filters")]
    public async Task<ActionResult> ReadWithFilters([FromQuery] string[] filters)
    {
        List<RecipeResponse> recipes = this.context.Recipes
            .Include(r => r.Ingredients)
                .ThenInclude(i => i.Article)
            .ToList()
            .Where(r => r.Categories != null && filters.All(f => r.Categories.Contains(f)))
            .Select(r => this.recipeMapper.RecipeToResponse(r))
            .ToList();

        return Ok(recipes);
    }

    [HttpGet("shoppingList")]
    public async Task<ActionResult> ShoppingList([FromQuery] string[] recipes)
    {
        List<ShoppingItem> shoppingList = new List<ShoppingItem>();
        List<string> missingRecipes = new List<string>();

        recipes.ToList().ForEach(async recipeName =>
        {
            Recipe recipe = await this.context.Recipes
                .Include(r => r.Ingredients)
                    .ThenInclude(i => i.Article)
                .FirstOrDefaultAsync(r => r.Name.Equals(recipeName));
            
            if (recipe == null)
            {
                missingRecipes.Add(recipeName);
                return;
            }
            
            recipe.Ingredients.ForEach(recipeIngredient =>
            {
                ShoppingItem item = shoppingList
                    .FirstOrDefault(list => list.Ingredient.Equals(recipeIngredient.Article.Name));

                if (item == null)
                {
                    shoppingList.Add(new ShoppingItem()
                    {
                        Ingredient = recipeIngredient.Article.Name,
                        UnconvertedQuantity = recipeIngredient.Quantity,
                        Unit = Utils.ParseStringAs<EQuantityType>(recipeIngredient.Article.Unit),
                    });
                }
                else
                {
                    item.UnconvertedQuantity += recipeIngredient.Quantity;
                }
            });
        });

        return Ok(missingRecipes.Count == 0 ? shoppingList : new
        {
            ShoppingList = shoppingList,
            Warning = "Missing recipes : " + string.Join(", ", missingRecipes),
        });
    }
}
