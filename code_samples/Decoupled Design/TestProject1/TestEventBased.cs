using EventBased;
using Program = EventBasedWithConfigure.Program;

[TestClass]
public class TestEventBased
{
    [TestMethod]
    public void TestConfigure()
    {
        var (aClass, foo) = Program.Configure();

        Assert.IsTrue(
            aClass.OnBaz.GetInvocationList().Contains(foo.Bar)
        );
    }


    [TestMethod]
    public void TestRaiseEvent()
    {
        var aClass = new AClass();
        var results = new List<string>();
        aClass.OnBaz += results.Add;

        aClass.Do();

        Assert.AreEqual(
            "Hello, World!",
            results.Single()
        );
    }
}