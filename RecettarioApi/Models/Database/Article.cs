using System.ComponentModel;

namespace RecettarioApi.Models.Database;

public class Article
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; } = EArticleCategory.None.ToString();
    public string Unit { get; set; } = EQuantityType.None.ToString();
    public string ImageUrl { get; set; }
}

public enum EArticleCategory
{
    [Description("Autre")]
    None,

    [Description("Fruits")]
    Fruit,
    [Description("Légumes")]
    Vegetable,
    [Description("Viandes")]
    Meat,
    [Description("Poissons")]
    Fish,
    [Description("Produits frais")]
    Fresh,
    [Description("Surgelés")]
    Frozen,
    [Description("Boissons")]
    Drink,
    [Description("Epiceries salées")]
    SaltEpicery,
    [Description("Epiceries sucrées")]
    SugarEpicery,
    [Description("Hygiène et beauté")]
    HygieneAndBeauty,
    [Description("Entretien et nettoyage")]
    Cleaning,
}

public enum EQuantityType
{
    [Description("")]
    None,
    
    Gram,
    Mililiter,

    [Description("Cuillères à café")]
    TeaSpoon,
}
