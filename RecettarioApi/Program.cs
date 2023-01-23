using Microsoft.EntityFrameworkCore;
using RecettarioApi;
using RecettarioApi.Controllers;
using RecettarioApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<RecipeMapper>();
builder.Services.AddDbContext<Context>(options =>
{
    options.UseInMemoryDatabase("database");
});

var app = builder.Build();

IServiceScope scope = app.Services.CreateScope();
Context context = scope.ServiceProvider.GetService<Context>();
await Startup.InitializeDatabase(context);

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
