using Microsoft.EntityFrameworkCore; 
using Domain.Entities; 
using Pomelo.EntityFrameworkCore.MySql;
namespace Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Trader> Traders { get; set; } = null!;
    public DbSet<StockOrder> StockOrders { get; set; } = null!;
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}
