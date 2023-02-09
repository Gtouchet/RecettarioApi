using RecettarioApi.Models.Database;
using System.Text.Json.Serialization;

namespace RecettarioApi.Models.Response;

public class ShoppingListResponse
{
    public List<ShoppingItem> ShoppingList { get; set; }
    public string Warning { get; set; }
}

public class ShoppingItem
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public string ImageUrl { get; set; }
    public float Quantity { get { return float.Parse(this.QuantityString.Split(" ")[0]); } }
    public string Unit { get { return string.Join(" ", this.QuantityString.Split(" ").Skip(1)); } }

    [JsonIgnore]
    public string QuantityString
    {
        get
        {
            return this.UnconvertedUnit switch
            {
                EQuantityType.None => this.UnconvertedQuantity.ToString(),
                EQuantityType.Gram => this.UnconvertedQuantity < 1_000 ?
                        this.UnconvertedQuantity + " g" :
                        (float)this.UnconvertedQuantity / 1000 + " Kg",
                EQuantityType.Mililiter => this.UnconvertedQuantity < 1_000 ?
                        this.UnconvertedQuantity + " ml" :
                        (float)this.UnconvertedQuantity / 1000 + " L",
                _ => this.UnconvertedQuantity + " " + Utils.GetEnumDescription(this.UnconvertedUnit).ToLower(),
            };
        }
    }
    [JsonIgnore]
    public EQuantityType UnconvertedUnit { get; set; }
    [JsonIgnore]
    public int UnconvertedQuantity { get; set; }
}
