using FinanceApplication.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace FinanceApplication.Dal.Concrete.Context;

public class FinanceAppDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=finance-app.database;Port=5432;Database=FinanceDb;Username=sa;Password=finance;Include Error Detail=true");
    }
    
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Request> Requests { get; set; }
    public DbSet<User> Users { get; set; }
}