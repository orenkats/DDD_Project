namespace Domain.Entities;

public class Trader
{
    public Guid Id { get; set; } // Primary key
    public string Name { get; set; } = null!;
    public decimal AccountBalance { get; set; }
    public string Email { get; set; } = null!;
    public List<StockOrder> Orders { get; set; } = new List<StockOrder>();

}
