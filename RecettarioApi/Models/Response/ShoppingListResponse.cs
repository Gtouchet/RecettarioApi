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
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public string Quantity
    {
        get
        {
            switch (this.Unit)
            {
                case EQuantityType.None: return this.UnconvertedQuantity.ToString();
                case EQuantityType.Gram: return this.UnconvertedQuantity < 1_000 ?
                        this.UnconvertedQuantity + " g" :
                        this.UnconvertedQuantity / 1000 + " Kg";
                case EQuantityType.Mililiter: return this.UnconvertedQuantity < 1_000 ? 
                        this.UnconvertedQuantity + " ml" :
                        this.UnconvertedQuantity / 1000 + " L";
                default: return this.UnconvertedQuantity + " " + Utils.GetEnumDescription(this.Unit).ToLower();
            }
        }
        set { }
    }

    [JsonIgnore]
    public EQuantityType Unit { get; set; }
    [JsonIgnore]
    public int UnconvertedQuantity { get; set; }
}
