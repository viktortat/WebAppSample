namespace WebApp.Tests.SampleTests;

//https://www.programmingwithwolfgang.com/xunit-getting-started/

/*
public class TimeFixture
{
    public TimeFixture()
    {
        DateTime = DateTime.Now;
    }

    public DateTime DateTime { get;  set; }
}

[CollectionDefinition("My TimeCollection")]
public class TimeCollection : ICollectionFixture<TimeFixture> 
{

}
[Collection("My TimeCollection")]
public class CollectionTests 
{
    private readonly ITestOutputHelper _oConsole;
    private readonly TimeFixture _timeFixture;

    public CollectionTests(ITestOutputHelper oConsole, TimeFixture timeFixture)
    {
        _oConsole = oConsole;
        _timeFixture = timeFixture;
    }
 
    [Fact]
    public void TestWithSameTimeFexture()
    {
        _oConsole.WriteLine($"{_timeFixture.DateTime}");
        Thread.Sleep(15000);
    }
    [Fact]
    public void Test2WithSameTimeFexture()
    {
        _oConsole.WriteLine($"{_timeFixture.DateTime}");
        Thread.Sleep(10000);
    }

}

*/