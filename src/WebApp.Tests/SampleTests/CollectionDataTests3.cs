namespace WebApp.Tests.SampleTests;

/*
public class TestDataGenerator2
{
    private IEnumerable<object[]> _enumerableImplementation;

    public static IEnumerable<object[]> GetNumbersFromDataGenerator()
    {
        yield return new object[] { 5, 1, 3, 9 };
        yield return new object[] { 7, 1, 5, 3 };
    }

    public static IEnumerable<object[]> GetFromDataGenerator()
    {
        yield return new object[]
        {
            new Product() {Id = 1, Name = "Pizza1", Price = 101, Description = "Desc1"},
            new Product() {Id = 2, Name = "Pizza2", Price = 102, Description = "Desc2"},
            new Product() {Id = 3, Name = "Pizza3", Price = 103, Description = "Desc3"}
        };
    }
    public static IEnumerable<object[]> GetFromDataGeneratorPizza()
    {
        yield return new object[] { new Product() { Id = 1, Name = "Pizza1", Price = 101, Description = "Desc1" } };
        yield return new object[] { new Product() { Id = 2, Name = "Pizza2", Price = 102, Description = "Desc2" } };
        yield return new object[] { new Product() { Id = 3, Name = "Pizza3", Price = 103, Description = "Desc3" } };
    }
}

public class ParameterizedTests
{
    private readonly ITestOutputHelper _oConsole;

    public ParameterizedTests(ITestOutputHelper oConsole)
    {
        _oConsole = oConsole;
    }

    public bool IsOddNumber(int number)
    {
        return number % 2 != 0;
    }

    public static IEnumerable<object[]> GetNumbers()
    {
        yield return new object[] { 5, 1, 3, 9 };
        yield return new object[] { 7, 1, 5, 3 };
    }

    [Theory]
    [MemberData(nameof(GetNumbers))]
    public void AllNumbers_AreOdd_WithMemberData(int a, int b, int c, int d)
    {
        Assert.True(IsOddNumber(a));
        Assert.True(IsOddNumber(b));
        Assert.True(IsOddNumber(c));
        Assert.True(IsOddNumber(d));
    }

    [Theory]
    [MemberData(nameof(TestDataGenerator2.GetNumbersFromDataGenerator), MemberType = typeof(TestDataGenerator2))]
    public void AllNumbers_AreOdd_WithMemberData_FromDataGenerator(int a, int b, int c, int d)
    {
        Assert.True(IsOddNumber(a));
        Assert.True(IsOddNumber(b));
        Assert.True(IsOddNumber(c));
        Assert.True(IsOddNumber(d));
    }

    [Theory]
    [MemberData(nameof(TestDataGenerator2.GetFromDataGenerator), MemberType = typeof(TestDataGenerator2))]
    public void All_WithMemberData_FromDataGenerator(Product a, Product b, Product c)
    {
        _oConsole.WriteLine($"{a.Id} # {b.Id}");
    }

    [Theory]
    [MemberData(nameof(TestDataGenerator2.GetFromDataGeneratorPizza), MemberType = typeof(TestDataGenerator2))]
    public void All_WithMemberData_FromDataGeneratorPizza(Product a)
    {
        _oConsole.WriteLine($"{a.Id} # {a.Name}");
    }

}

*/