using Microsoft.AspNetCore.Mvc;
using RecettarioApi.Models;
using RecettarioApi.Models.Database;
using RecettarioApi.Models.Response;

namespace RecettarioApi.Controllers;

#pragma warning disable CS1998

[ApiController]
[Route("[controller]")]
public class ArticleController : ControllerBase
{
    private readonly Context context;
    private readonly Mapper mapper;

    public ArticleController(
        Context context,
        Mapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }
    
    [HttpGet]
    public async Task<ActionResult> Read(Guid? id = null)
    {
        IQueryable<Article> query = this.context.Articles;

        if (id != null)
        {
            query = query.Where(r => r.Id.Equals(id));
        }

        List<ArticleResponse> articles = query
            .Select(a => this.mapper.ArticleToResponse(a))
            .ToList()
            .OrderBy(a => a.Category)
            .ToList();
        
        return id == null ?
            Ok(articles) : articles.Count > 0 ?
                Ok(articles.First()) : NotFound("L'article n'existe pas");
    }

    [HttpGet("categories")]
    public async Task<ActionResult> ReadWithCategory([FromQuery] string[] categories)
    {
        List<ArticleResponse> articles = this.context.Articles
            .ToList()
            .Where(a => a.Category != null && categories.Any(a.Category.Contains))
            .Select(a => this.mapper.ArticleToResponse(a))
            .ToList();

        return Ok(articles);
    }
}
