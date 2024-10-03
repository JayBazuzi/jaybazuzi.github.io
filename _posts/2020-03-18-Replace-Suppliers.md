---
layout: post
title: _Replace Supplier with Supplies_ and friends
---

Here are three very specific uses of Extract Method on a function that takes an unwieldy object as an input, to help make code cleaner and easier to test. I learned these from Arlo Belshee.

# Leave Loaders Behind

## Context

The first lines of the function pull some values out of the large passed-in object. The rest of the function does not access the large object again.

```
F(Database database)
{
    var a = database.get...
    var b = database.get...
    var c = database.get...

    // do stuff with a/b/c
}
```

## How to do it

Extract Method, for all of the method except for the lines that extract those bits off data.

```
F(database)
{
    var a = database.get...
    var b = database.get...
    var c = database.get...
    F(a, b, c)
}

F(a, b, c)
{
    // do stuff with a/b/c
}
```

The new function gets the same name as the old function (it's an overload), since it does the same thing.

## Advanced

- Sometimes at the end of the method, the result of a method gets packaged into a large object to be returned. Sometimes you can leave that result-packing behind in the same way. 

- Inspect callers; some will be happier calling the new overload.

- Consider whether a subset of parameters relate to each other in a way that would make sense as a new object (Extract Parameter Object). Then consider moving this new method onto the new object.

# Replace Supplier with Supplies

## Context

Similar to Leave Loaders Behind, the function only uses a subset of the values in an object that is passed in, but not all at the top of the function. 

```
F(Database database)
{
    // some code...

    var a = database.get...

    // more code...

    var b = database.get...

    // still more code

    var c = database.get...

    // yet more code...
}
```


## How to do it

First incrementally refactor to bring all the object reads to the top of the function

```
F(Database database)
{
    var a = database.get...
    var b = database.get...
    var c = database.get...

    // some code...

    // more code...

    // still more code

    // yet more code...
}
```

Then apply Leave Loaders Behind in the same way.

Be sure to verify that reordering these statements doesn't change behavior, for example if the retrieved value changes as the function executes.

# Replace Suppliers with Thunks

## Context

Similar to Replace Supplier with Supplies, but you can't be sure that retrieved values aren't changing, or the function is calling a setter or other state-modifying code.

## How to do it

Wrap each troublesome statement/expression in an immediately-executed lambda:

```
F(Database database)
{
    // some code...

    var a = (() -> database.get...)()

    // more code...

    var b = (() -> database.get...)()

    // still more code

    var c = (() -> database.get...)()

    // yet more code...
}
```

Then Extract Local Variable on these lambdas:

```
F(Database database)
{
    // some code...

    var getA = (() -> database.get...)
    var a = getA()

    // more code...

    var getB = (() -> database.get...)
    var b = getB()

    // still more code

    var getC = (() -> database.get...)
    var c = getC()

    // yet more code...
}
```

Then reorder statements:

```
F(Database database)
{
    var getA = (() -> database.get...)
    var getB = (() -> database.get...)
    var getC = (() -> database.get...)

    // some code...

    var a = getA()

    // more code...

    var b = getB()

    // still more code

    var c = getC()

    // yet more code...
}
```

Then extract method:

```
F(Database database)
{
    var getA = (() -> database.get...)
    var getB = (() -> database.get...)
    var getC = (() -> database.get...)

    F(getA, getB, getC)
}

F(getA, getB, getC)
{
    // some code...

    var a = getA()

    // more code...

    var b = getB()

    // still more code

    var c = getC()

    // yet more code...
}
```

It's OK to name the new function with an `_Impl` suffix and keep it `private`, if you don't think it's a natural API to be exposing from this object. Consider both options.

## Advanced

A subset of these lambdas might become methods on a new object, possibly a facade.
