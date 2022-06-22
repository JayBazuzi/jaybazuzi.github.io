---
layout: post
title: Decoupled Design with Events
---

When we try to apply unit testing, a question that often comes up is "how can we test this difficult-to-test code?". That question has an important built-in assumption, that the code as currently written should not change for the sake of testing, and shall be tested as-is. That's unfortunate, since a big potential benefit of unit testing is that it helps you detect and repair harmful coupling. Let's look specifically at some options for decoupling for testability.

<!-- snippet: direct-call -->
<a id='snippet-direct-call'></a>
```cs
public static class Program
{
    public static void Main()
    {
        var aClass = new AClass();
        aClass.Do();
    }
}

public class AClass
{
    private readonly Foo _foo = new Foo();

    public void Do()
    {
        // ...
        this._foo.Bar("Hello, World!");
        // ...
    }
}

; public class Foo
{
    public void Bar(string message)
    {
        Console.Write(message);
    }
}
```
<sup><a href='/code_samples/Decoupled Design/1. Direct Call/Program.cs#L3-L33' title='Snippet source file'>snippet source</a> | <a href='#snippet-direct-call' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

We want to test that the right output is printed at the right time. One option is to override `Console.Out` to capture that result:

<!-- snippet: end-to-end-test -->
<a id='snippet-end-to-end-test'></a>
```cs
using var consoleOutput = new StringWriter();
Console.SetOut(consoleOutput);

Program.Main();

Assert.AreEqual("Hello, World!", consoleOutput.ToString());
```
<sup><a href='/code_samples/Decoupled Design/TestProject1/TestAll.cs#L64-L71' title='Snippet source file'>snippet source</a> | <a href='#snippet-end-to-end-test' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

We're using `Console` here but this approach could work with every external dependency - the file system, HTTP, a database, etc.

There are some things to like about this approach: Test coverage is excellent. Internal refactoring will not break tests. For code that makes over-the-network calls, network or server issues will not cause tests to fail.

This is a toy example; in the real world if we approached all testing this way -- by injecting/hijacking at the perimeter of the system to write only edge-to-edge tests -- our tests would lack some of the qualities we find valuable. Tests will be hard to read because they do a lot of unrelated setup. A single bug might cause many tests to fail. A single failing test could be caused by problems anywhere in the system. These tests are difficult to live with.

# Dependency Inversion

A popular alternative is to add a layer of indirection and use it to inject a test double. Suppose we refactor to extract an interface:

<!-- snippet: dependency-inversion-interface -->
<a id='snippet-dependency-inversion-interface'></a>
```cs
public interface IFoo
{
    void Bar(string message);
}

public class Foo : IFoo
{
    public void Bar(string message)
    {
        Console.Write(message);
    }
}
```
<sup><a href='/code_samples/Decoupled Design/Dependency Inversion/Program.cs#L3-L16' title='Snippet source file'>snippet source</a> | <a href='#snippet-dependency-inversion-interface' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

And then parameterize:

<!-- snippet: dependency-inversion-usage -->
<a id='snippet-dependency-inversion-usage'></a>
```cs
public class AClass
{
    private readonly IFoo _foo;

    public AClass(IFoo foo)
    {
        _foo = foo;
    }

    public void Do()
    {
        // ...
        _foo.Bar("Hello, World!");
        // ...
    }
}

public static class Program
{
    public static void Main()
    {
        var aClass = new AClass(new Foo());
        aClass.Do();
    }
}
```
<sup><a href='/code_samples/Decoupled Design/Dependency Inversion/Program.cs#L18-L44' title='Snippet source file'>snippet source</a> | <a href='#snippet-dependency-inversion-usage' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Which allows us to substitute a test double:

<!-- snippet: dependency-inversion-test -->
<a id='snippet-dependency-inversion-test'></a>
```cs
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
```
<sup><a href='/code_samples/Decoupled Design/TestProject1/TestDependencyInversion.cs#L11-L35' title='Snippet source file'>snippet source</a> | <a href='#snippet-dependency-inversion-test' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

I like that this lets me test `AClass` independently.

We might still use the `Console.SetOut()` approach to test `Foo`, but now we only have to do that for a smaller chunk of the program instead of the whole. That's a big improvement.

Tests can mow be more focused, making them easier to read, write, diagnose, and maintain. Cool.

We have lost all coverage of `Main()`. Maybe that's acceptable, since it is quite small and simple. The only way to get that cverage back would be whole-system testing like the original example.

My design sense is annoyed that we have an interface that only exists for the purpose of testing, and that we have a class/interface pair with the same name (`Foo`/`IFoo`). These are code smells. There is indirection without abstraction.

The coupling between `AClass` and `Foo` is reduced, but not completely eliminated. For example, if we wanted to rename `Bar()` we'd need to update both.

## Dependency Injection / Service Locator

A popular adjust to dependency *inversion*, especially in large software systems is dependency *injection*. Oh, the sweet, sweet siren song of Service Locator.

Instead of having `Main()` instantiating objects with the correct configuration, we might have something like:

<!-- snippet: dependency-injection-main -->
<a id='snippet-dependency-injection-main'></a>
```cs
public static void Main()
{
    var services = new Services();
    services.Add<IFoo>(new Foo());

    var aClass = services.CreateObject<AClass>();

    aClass.Do();
}
```
<sup><a href='/code_samples/Decoupled Design/Dependency Injection/Program.cs#L61-L71' title='Snippet source file'>snippet source</a> | <a href='#snippet-dependency-injection-main' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

`Services` would detect that `AClass` constructor takes an `IFoo` parameter, find an `IFoo`-implementing type in its collection of services, and pass it to the ctor.

In large systems, this approach can be *very* tempting, because it "solves" the problem of plumbing data all over the place. If I need to make `AClass`, I don't have to figure out where to find `Foo`, it just works. Like magic.

Let's call it what it is. We're making globals (bad) without it looking like we're making globals (worse) and making dependencies less visible (also bad). Do not like.

### Testing Configuration

Actually, there is one characteristic of this approach that I *do* particularly like: it is possible to write a test for the configuration:

<!-- snippet: service-locator-approval-test -->
<a id='snippet-service-locator-approval-test'></a>
```cs
var services = new Services();
services.Add<IFoo>(new Foo());

Approvals.Verify(services);
```
<sup><a href='/code_samples/Decoupled Design/TestProject1/TestDependencyInversion.cs#L44-L49' title='Snippet source file'>snippet source</a> | <a href='#snippet-service-locator-approval-test' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

(I'm assuming it has a nice `.ToString()` that prints its configuration in a human-friendly format. Or we write a formatter. Point is, this needs to be good for humans.)


### Dynamic Configuration

In almost every codebase I've worked with, the configuration was known at compile time. Occasionally, though, some aspect of configuration might need to be adjusted in production or at runtime. I think the details are off-topic for this article, but I acknowledge the need is real. Let's come back to that another time.

# Event-based

Can we reduce coupling further, and how does that affect tests? Suppose we replace the interface with an event*, and make the top-level orchestrator subscribe the event:

<!-- snippet: event-based -->
<a id='snippet-event-based'></a>
```cs
public class AClass
{
    public Action<string> OnBaz = delegate { };

    public void Do()
    {
        // ...
        OnBaz("Hello, World!");
        // ...
    }
}

public static class Program
{
    public static void Main()
    {
        var foo = new Foo();
        var aClass = new AClass();
        aClass.OnBaz += foo.Bar;

        aClass.Do();
    }
}
```
<sup><a href='/code_samples/Decoupled Design/Event Based/Program.cs#L11-L35' title='Snippet source file'>snippet source</a> | <a href='#snippet-event-based' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

We can test that `AClass` raises the event at the right time, like this:

<!-- snippet: event-based-test -->
<a id='snippet-event-based-test'></a>
```cs
var aClass = new AClass();
var results = new List<string>();
aClass.OnBaz += results.Add;

aClass.Do();

Assert.AreEqual(
    "Hello, World!",
    results.Single()
);
```
<sup><a href='/code_samples/Decoupled Design/TestProject1/TestEventBased.cs#L23-L34' title='Snippet source file'>snippet source</a> | <a href='#snippet-event-based-test' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

This test is basically accomplishing the same thing as the mock-based test in the previous section, but with an event instead of an interface, and without a mock.

Now there is even less coupling between `Foo` and `AClass`. They don't know about each other. To our previous example, we could rename `Bar` without changing `AClass`.

Unfortunately we have lost even more code coverage - the code in `Main()` is doing more than before. Luckily we can extract a method:

<!-- snippet: event-based-configure -->
<a id='snippet-event-based-configure'></a>
```cs
public static (AClass, Foo) Configure()
{
    var foo = new Foo();
    var aClass = new AClass();
    aClass.OnBaz += foo.Bar;
    return (aClass, foo);
}

public static void Main()
{
    var (aClass, _) = Configure();
    aClass.Do();
}
```
<sup><a href='/code_samples/Decoupled Design/Event Based with Configure/Program.cs#L25-L39' title='Snippet source file'>snippet source</a> | <a href='#snippet-event-based-configure' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

and write this test:

<!-- snippet: event-based-configure-test -->
<a id='snippet-event-based-configure-test'></a>
```cs
var (aClass, foo) = Program.Configure();

Assert.IsTrue(
    aClass.OnBaz.GetInvocationList().Contains(foo.Bar)
);
```
<sup><a href='/code_samples/Decoupled Design/TestProject1/TestEventBased.cs#L10-L16' title='Snippet source file'>snippet source</a> | <a href='#snippet-event-based-configure-test' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

# Decoupled Design

In his article on [Decoupled Design](https://arlobelshee.com/decoupled-design/) Arlo said:

> A system has its static structure and its run-time structure.

This phrasing confused me for a long time. I think I understand it now.

In the original code we started with, the static structure and run-time structure were the same:
- `AClass` depends on `Foo` which writes to the console

In the second example, using Dependency Injection, the static structure becomes:
-  `AClass` uses `IFoo`
-  `Foo` implements `IFoo` and writes to the console
-  `Main()` ties them together

In the third example, using events, the static structure becomes:
- `AClass` is a thing
- `Foo` is a thing that writes to the console
- `Initialize()` ties them together
Arlo goes on to say:

> An application also has two relevant time frames: initialization and running 

Initialization is handled by `Initialize()` which has no side effects and so is easy to test.

We can test each part in isolation. Everything that happens during Running is already tested separately, except for `Main()` which is *very* simple, and would be very hard to get wrong and still compile.

# Cross-component interaction

The less-coupled options described in this article are much less useful if the components involved need to be changed together: if changing one means you (probably) need to change the other and (definitely) need to test them together to ensure that they cooperate correctly. One way of describing this is that the features of the system are "emergent phenomena". In that case, the so-called decoupling strategies described here make the system worse - it becomes *harder* to modify. Finding the correct "cleaving points" is really important, but outside of the scope of this article. We'll come back to that another time.

# Footnote: events

*I mean an "event" in the general sense (this code tells the ether that something has happened, but doesn't know who is listening, if anyone), not in the [C# sense](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/event).
