using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Store : BaseModel
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public double? PricePerWeight { get; set; }
    public double? MinimumDropOffWeight { get; set; }
    
}