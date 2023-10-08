namespace WebApp.Api.Models;

public class Customer
{
    public int Id { get; set; }
    public string? CompanyName { get; set; }
    public string? Phone { get; set; }
    public string? ContactName { get; set; }
    public int AddressId { get; set; }
    public Address? Address { get;set;}
    public IEnumerable<Order>? Orders {get;set;}
}

public class Customer2
{
    public int Id { get; set; }
    public int Age { get; set; }
    public Gender Gender { get; set; }
    public string? FamilyName { get; set; }
    public List<string>? Names { get; set; }
    public IEnumerable<Book>? Books { get; set; }
    public string? FullName { get; set; }
}
public class Book
{
    public string? Title { get; set; }
    public decimal Price { get; set; }
    public double Rating { get; set; }
}
public enum Gender
{
    Male,
    Female,
    Unknown,
}
public class Person
{
    public string? FullName { get; set; }
    public byte Age { get; set; }
    public string? Phone { get; set; }
    public Work? Work { get; set; }
}
public class Work
{

    public string? Company { get; set; }
    public string? Position { get; set; }
}