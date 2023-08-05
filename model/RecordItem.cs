using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class RecordItem : BaseModel
{
    public string? Name { get; set; }
    public LaundryRecord? Record { get; set; }
    public int? RecordId { get; set; }
    public double? Qty { get; set; }
}