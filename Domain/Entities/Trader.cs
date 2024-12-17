namespace Domain.Entities;

public class Trader
{
    public Guid Id { get; set; } // Primary key
    public string Name { get; set; } = null!;
    public decimal AccountBalance { get; set; }
    
    // Changed to List<StockOrder> for indexing and easier manipulation
    public List<StockOrder> Orders { get; set; } = new List<StockOrder>();

}
