using Microsoft.EntityFrameworkCore;
using WebApp.Api.Data;
using WebApp.Api.Models;

namespace WebApp.Api.Services;

public interface IPizzaService
{
    Task<List<Product>> GetAll();
    Task<ApiResult<Product>> Get(int id);
    Task<ApiResult<Product>> Create(Product product);
    Task<ApiResult<Product>> Update(Product product);
    Task<ApiResult<Product>> Delete(int id);
}

public class PizzaService : IPizzaService
{
    private readonly ApplicationDbContext _dbContext;

    public PizzaService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Product>> GetAll() => await _dbContext.Pizzas.ToListAsync();

    public async Task<ApiResult<Product>> Get(int id)
    {
        var pizza = await _dbContext.Pizzas.FindAsync(id);
        if (pizza is null)
            return ApiResult<Product>.Failure(errorMessage: $"{id} not found", stCode: StatusCodes.Status404NotFound);
        //return new ApiResult<Product>(false, errorMessage:$"{id} not found");
        return new ApiResult<Product>(true, pizza);
    }

    public async Task<ApiResult<Product>> Create(Product product)
    {
        var ent = await _dbContext.Pizzas.AddAsync(product);
        await _dbContext.SaveChangesAsync();
        product.Id = ent.Entity.Id;
        return ApiResult<Product>.Success(product);
    }

    public async Task<ApiResult<Product>> Update(Product product)
    {
        var pizzaItem = await _dbContext.Pizzas.FindAsync(product.Id);
        if (pizzaItem is null) return new ApiResult<Product>(false, errorMessage: $"{product.Id} not found");
        pizzaItem.Name = product.Name;
        pizzaItem.Description = product.Description;
        pizzaItem.Price = product.Price;
        await _dbContext.SaveChangesAsync();
        return new ApiResult<Product>(true, pizzaItem);
    }

    public async Task<ApiResult<Product>> Delete(int id)
    {
        var todo = await _dbContext.Pizzas.FindAsync(id);
        if (todo is null)
        {
            return new ApiResult<Product>(false);
        }
        _dbContext.Pizzas.Remove(todo);
        await _dbContext.SaveChangesAsync();
        return new ApiResult<Product>(true); ;
    }
}