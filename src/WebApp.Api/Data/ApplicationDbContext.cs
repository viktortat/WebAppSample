using Microsoft.EntityFrameworkCore;
using WebApp.Api.Models;

namespace WebApp.Api.Data;

public class ApplicationDbContext : DbContext
{
    private const string DefaultUserName = "Anonymous";

    //private readonly ILogger<ApplicationDbContext> _logger;
    //private readonly Fakers _fakers;

    //public ApplicationDbContext(
    //    DbContextOptions options,
    //    ILogger<ApplicationDbContext> logger,
    //    Fakers fakers
    //    ) : base(options)
    //{
    //    _logger = logger;
    //    _fakers = fakers;
    //}
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
        LastSaveChangesResult = new SaveChangesResult();
    }

    public DbSet<Product> Pizzas { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            DbSaveChanges();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        catch (System.Exception exception)
        {
            LastSaveChangesResult.Exception = exception;
            return Task.FromResult(0);
        }
    }

    private void DbSaveChanges()
    {
        //			
        var createdEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added);
        foreach (var entry in createdEntries)
        {
            if (!(entry.Entity is IAuditable))
            {
                continue;
            }

            var creationDate = DateTime.Now.ToUniversalTime();
            var userName = entry.Property("creator_id").CurrentValue == null
                ? DefaultUserName
                : entry.Property("creator_id").CurrentValue;
            var update_date = entry.Property("update_date").CurrentValue;
            var create_date = entry.Property("create_date").CurrentValue;
            if (create_date != null)
            {
                if (DateTime.Parse(create_date.ToString()).Year > 1970)
                {
                    entry.Property("create_date").CurrentValue = ((DateTime)create_date).ToUniversalTime();
                }
                else
                {
                    entry.Property("create_date").CurrentValue = creationDate;
                }
            }
            else
            {
                entry.Property("create_date").CurrentValue = creationDate;
            }

            if (update_date != null)
            {
                if (DateTime.Parse(update_date.ToString()).Year > 1970)
                {
                    entry.Property("update_date").CurrentValue = ((DateTime)update_date).ToUniversalTime();
                }
                else
                {
                    entry.Property("update_date").CurrentValue = creationDate;
                }
            }
            else
            {
                entry.Property("update_date").CurrentValue = creationDate;
            }

            entry.Property("creator_id").CurrentValue = userName;
            entry.Property("updater_id").CurrentValue = userName;

            LastSaveChangesResult.AddMessage($"ChangeTracker has new entities: {entry.Entity.GetType()}");
        }

        var updatedEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified);
        foreach (var entry in updatedEntries)
        {
            if (entry.Entity is IAuditable)
            {
                var userName = entry.Property("updater_id").CurrentValue == null
                    ? DefaultUserName
                    : entry.Property("updater_id").CurrentValue;
                entry.Property("update_date").CurrentValue = DateTime.Now.ToUniversalTime();
                entry.Property("updater_id").CurrentValue = userName;
            }

            LastSaveChangesResult.AddMessage($"ChangeTracker has modified entities: {entry.Entity.GetType()}");
        }
    }
    public SaveChangesResult LastSaveChangesResult { get; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //var addresses = _fakers.GetAddressGenerator().Generate(50);

        //modelBuilder.Entity<Address>()
        //    .HasData(addresses);
        //var customers = _fakers.GetCustomerGenerator(false).Generate(50);

        //for (var x = 0; x < customers.Count(); ++x)
        //{
        //    customers[x].AddressId = addresses[x].Id;
        //}

        //modelBuilder.Entity<Customer>()
        //    .HasData(customers);

    }
}

