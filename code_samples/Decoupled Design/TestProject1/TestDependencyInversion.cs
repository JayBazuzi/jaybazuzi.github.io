using DependencyInversion;

[TestClass]
public class TestDependencyInversion
{
// begin-snippet: dependency-inversion-test
    [TestMethod]
    public void Test()
    {
        var fooMock = new FooMock();
        var aClass = new AClass(fooMock);

        aClass.Do();

        Assert.AreEqual(
            "Hello, World!",
            fooMock.Results.Single()
        );
    }

    private class FooMock : IFoo
    {
        public readonly List<string> Results = new();

        void IFoo.Bar(string message)
        {
            Results.Add(message);
        }
    }
// end-snippet
}
