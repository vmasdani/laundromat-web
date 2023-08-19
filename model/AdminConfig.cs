public class AdminConfig : BaseModel
{
    public string? SuperAdminPassword { get; set; }
    public string? JwtSecret { get; set; }


}