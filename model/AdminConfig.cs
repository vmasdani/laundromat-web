public class AdminConfig : BaseModel
{
    public string? SuperAdminPassword { get; set; }
    public string? JwtSecret { get; set; }

    public Store? DefaultStore { get; set; }
    public int? DefaultStoreId { get; set; }


}