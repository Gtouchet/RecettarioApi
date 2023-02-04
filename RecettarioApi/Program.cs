using Microsoft.EntityFrameworkCore;
using RecettarioApi;
using RecettarioApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<Mapper>();
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
