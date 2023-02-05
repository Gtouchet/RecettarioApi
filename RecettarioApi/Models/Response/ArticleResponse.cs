using RecettarioApi.Models.Database;

namespace RecettarioApi.Models.Response;

public class ArticleResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public string Unit { get; set; }
    public string ImageUrl { get; set; }
}
