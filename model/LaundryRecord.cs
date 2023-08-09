using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class LaundryRecord : BaseModel
{
    public string? Name { get; set; }
    public List<RecordItem>? RecordItems { get; set; }

    public Customer? Customer { get; set; }
    public int? CustomerId { get; set; }
    public Customer? Store { get; set; }
    public int? StoreId { get; set; }
    public string? Remark { get; set; }
    public double? Weight { get; set; }
    public double? PriceSnapshot { get; set; }
    public bool? IsDiscount { get; set; }
    public double? DiscountPrice { get; set; }
    public LaundryRecordStatus? Status { get; set; }
    public bool? Paid { get; set; }

}