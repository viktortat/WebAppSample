using Microsoft.EntityFrameworkCore;
using WebApp.Api.Models;

namespace WebApp.Tests.Data;

public class MockDb : IDbContextFactory<PizzaDbContext>
{
    public PizzaDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<PizzaDbContext>()
            .UseInMemoryDatabase($"InMemoryTestDb-{DateTime.Now.ToFileTimeUtc()}")
            .Options;
        return new PizzaDbContext(options);
    }
}

public class PizzaDbContext : DbContext
{
    //public DbSet<Product> Todos => Set<Product>();
    public DbSet<Product> Pizzas { get; set; }
    public PizzaDbContext(DbContextOptions<PizzaDbContext> options) : base(options) { }
}