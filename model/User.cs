using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class User : BaseModel
{
    public string? Username { get; set; }

    // [JsonIgnore]
    public string? Password { get; set; }

    [NotMapped]
    public string? PasswordStr
    {
        get{return Password;}
        set
        {
            if (value != null && value != "")
                this.Password = value;
        }
    }
}