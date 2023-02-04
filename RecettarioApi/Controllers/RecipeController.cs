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
    private readonly Mapper mapper;

    public RecipeController(
        Context context,
        Mapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
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
            .Select(r => this.mapper.RecipeToResponse(r))
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
            .Select(r => this.mapper.RecipeToResponse(r))
            .ToList();

        return Ok(recipes);
    }

    [HttpGet("shoppingList")]
    public async Task<ActionResult> ShoppingList([FromQuery] Guid[] ids)
    {
        List<ShoppingItem> shoppingList = new List<ShoppingItem>();
        List<Guid> missingRecipes = new List<Guid>();

        ids.ToList().ForEach(async id =>
        {
            Recipe recipe = await this.context.Recipes
                .Include(r => r.Ingredients)
                    .ThenInclude(i => i.Article)
                .FirstOrDefaultAsync(r => r.Id.Equals(id));
            
            if (recipe == null)
            {
                missingRecipes.Add(id);
                return;
            }
            
            recipe.Ingredients.ForEach(recipeIngredient =>
            {
                ShoppingItem item = shoppingList
                    .FirstOrDefault(list => list.Name.Equals(recipeIngredient.Article.Name));

                if (item == null)
                {
                    shoppingList.Add(new ShoppingItem()
                    {
                        Name = recipeIngredient.Article.Name,
                        UnconvertedQuantity = recipeIngredient.Quantity,
                        ImageUrl = recipeIngredient.Article.ImageUrl,
                        Unit = Utils.ParseStringAs<EQuantityType>(recipeIngredient.Article.Unit),
                    });
                }
                else
                {
                    item.UnconvertedQuantity += recipeIngredient.Quantity;
                }
            });
        });

        return Ok(new ShoppingListResponse()
        {
            ShoppingList = shoppingList,
            Warning = missingRecipes.Count > 0 ? "Missing recipes : " + string.Join(", ", missingRecipes) : null,
        });
    }
}
