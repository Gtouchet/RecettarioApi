using Microsoft.EntityFrameworkCore;
using RecettarioApi.Models.Database;

namespace RecettarioApi.Models;

public class Context : DbContext
{
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<RecipeIngredient> RecipeIngredients { get; set; }

    public Context(DbContextOptions options) : base(options)
    {

    }
}
