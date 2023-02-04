using Microsoft.AspNetCore.Mvc;
using RecettarioApi.Models;

namespace RecettarioApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DatabaseController : ControllerBase
{
    private readonly Context context;

    public DatabaseController(Context context)
    {
        this.context = context;
    }
    
    [HttpPost("refresh")]
    public async Task<ActionResult> Refresh()
    {
        this.context.Articles.RemoveRange(this.context.Articles);
        this.context.Recipes.RemoveRange(this.context.Recipes);
        this.context.RecipeIngredients.RemoveRange(this.context.RecipeIngredients);
        await this.context.SaveChangesAsync();
        
        await Startup.InitializeDatabase(this.context);

        return Ok();
    }
}
