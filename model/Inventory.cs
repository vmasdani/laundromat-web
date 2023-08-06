public class Inventory : BaseModel
{
    public string? Name { get; set; }
    public Store? Store { get; set; }
    public int? StoreId { get; set; }
    public Item? Item { get; set; }
    public int? ItemId { get; set; }
    public InventoryTransactionMode? Mode { get; set; }
    public double? Qty { get; set; }
    public string? Remark { get; set; }
}