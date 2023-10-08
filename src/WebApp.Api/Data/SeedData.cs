using WebApp.Api.Data;
using WebApp.Api.Data.Fakers;
using WebApp.Api.Models;

public static class SeedData
{
    public static async Task Initialize(ApplicationDbContext db, int generateCount = 50)
    {
        if (!db.Pizzas.Any())
        {
            db.Pizzas.AddRange(
                new Product { Id = 1, Name = "Pepperoni", Description = "Classic Pepperoni Product", Price = 155, creator_id = "init" },
                new Product { Id = 2, Name = "От шефа", Description = "много сыра, помидоров, колбасы", Price = 120, creator_id = "init" },
                new Product { Id = 3, Name = "Hawaiian", Price = 130, creator_id = "init" },
                new Product { Id = 4, Name = "Margherita", Price = 175, creator_id = "init" },
                new Product { Id = 5, Name = "Veggie Supreme", Price = 180, creator_id = "init" },
                new Product { Id = 6, Name = "Meat Lover's", Price = 230, creator_id = "init" },
                new Product { Id = 7, Name = "BBQ Chicken", Price = 98, creator_id = "init" },
                new Product { Id = 8, Name = "Four Cheese", Price = 127, creator_id = "init" },
                new Product { Id = 9, Name = "Supreme", Price = 210, creator_id = "init" },
                new Product { Id = 10, Name = "Buffalo Chicken", Price = 178, creator_id = "init" }
            );
            await db.SaveChangesAsync();
        }

        if (!db.Addresses.Any() && !db.Customers.Any())
        {
            var fakers = new Fakers();

            var addresses = fakers.GetAddressGenerator().Generate(generateCount );
            var customers = fakers.GetCustomerGenerator(false).Generate(generateCount);

            for (var x = 0; x < customers.Count(); ++x)
            {
                customers[x].AddressId = addresses[x].Id;
            }
            db.Addresses.AddRange(addresses);
            db.Customers.AddRange(customers);
            await db.SaveChangesAsync();
        }
    }
}
