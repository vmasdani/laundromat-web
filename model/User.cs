using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class User : BaseModel
{
    public string? Username { get; set; }
    public string? Name { get; set; }
    public string? Role { get; set; }
    

    [JsonIgnore]
    public string? Password { get; set; }

    [NotMapped]
    public string? PasswordStr
    {
        internal get;
        set;
    }
}