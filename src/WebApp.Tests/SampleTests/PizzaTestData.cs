namespace WebApp.Tests.SampleTests;
//https://www.programmingwithwolfgang.com/xunit-getting-started/

/*
public class PizzaTestData
{
    public static IEnumerable<object[]> TestData
    {
        get
        {
            yield return new object[] { 1, "Pizza1" };
            yield return new object[] { 2, "Pizza2" };
            yield return new object[] { 3, "Pizza3" };
        }
    }
    public List<Product> TestPizzaData
    {
        get
        {
            return new List<Product>
            {
                new Product() {Id = 1, Name = "Pizza1", Price = 101, Description = "Desc1"},
                new Product() {Id = 2, Name = "Pizza2", Price = 102, Description = "Desc2"},
                new Product() {Id = 3, Name = "Pizza3", Price = 103, Description = "Desc3"}
            };
        }
    }
}
public class TestDataGenerator : IEnumerable<object[]>
{
    private readonly List<object[]> _data = new List<object[]>
    {
        new object[] {5, 1, 3, 9},
        new object[] {7, 1, 5, 3}
    };

    public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class PizzaTestData2Attribute : DataAttribute
{
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        yield return new object[] { 21, "Pizza21" };
        yield return new object[] { 22, "Pizza22" };
        yield return new object[] { 23, "Pizza23" };
    }
}
public class ExsternalDataTestData
{
    public static IEnumerable<object[]> GetDataFromCsv
    {
        get
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var lines = File.ReadAllLines(Path.Combine(path, "TestDataCsv.csv"));
            return lines.Select(line => line.Split(',').Cast<object>().ToArray()).ToList();
        }
    }
}
public class CollectionDataTests
{
    private readonly ITestOutputHelper _oConsole;

    public CollectionDataTests(ITestOutputHelper oConsole)
    {
        _oConsole = oConsole;
    }

    [Theory]
    [ClassData(typeof(TestDataGenerator))]
    public void AllNumbers_AreOdd_WithClassData(int a, int b, int c, int d)
    {
        _oConsole.WriteLine($"{a} # {b} # {c} # {d}");
    }


    [Theory]
    [MemberData(nameof(PizzaTestData.TestPizzaData), MemberType = typeof(Product))]
    public void Test1WithData4(int Id, string Name)
    {
        _oConsole.WriteLine($"{Id} # {Name}");
        var dd = 1;
        dd.Should().Be(1);
    }

    [Theory]
    [MemberData(nameof(PizzaTestData.TestData), MemberType = typeof(PizzaTestData))]
    public void Test1WithData(int id, string name)
    {
        _oConsole.WriteLine($"{id} # {name}");
        var dd = 1;
        dd.Should().Be(1);
    }

    [Theory]
    [PizzaTestData2]
    public void Test2WithPizzaTestData2(int id, string name)
    {
        _oConsole.WriteLine($"{id} # {name}");
        var dd = 1;
        dd.Should().Be(1);
    }

    [Theory]
    [MemberData(nameof(ExsternalDataTestData.GetDataFromCsv), MemberType = typeof(ExsternalDataTestData))]
    public void Test3TestDataFromCsv(int id, string name)
    {
        _oConsole.WriteLine($"{id} # {name}");
        var dd = 1;
        dd.Should().Be(1);
    }
}

*/