using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class RecordItem : BaseModel
{
    public string? Name { get; set; }
}