---
layout: post
title: The problem with Windows Batch files (and what we can do about it)
---

Windows Batch files are the worst.
The almost always have bugs that we won't notice even if we are very careful.
And yet, they are unavoidable - the only solution for certain problems in certain contexts.
How can we engineer our way to a good place?

# What is a Batch file?

A Batch file, sometimes called a "Batch script", is a text file that contains a sequence of Windows CMD commands, like `cd` and `dir`.


# What's wrong with Batch?

There are so many "hazards" or "pitfalls", where even a careful developer is likely to make a mistake, and that mistake is unlikely to be caught even with diligent testing.
Here are some examples:

## Spaces in variables:

```bat
set X=Y
```

If there's a space at the end of the line, then `X` will include that space.
A good way to avoid this problem is to always use quotes, but not in the way you'd probably guess at first:

```bat
set X="Y"       # no, the value of X will include the quotes!
set "X=Y"       # yes, this works
```

## Abort on error

If a batch file is meant to run a series of commands, and we want to abort on the first one that fails.
In Bash we can use `set -e` abort on error, but in Batch we have to remember to check the result of every command.
Even if we're being diligent, it's easy to miss one statement, and we probably won't notice in testing if we don't have a way to cause each command to fail on purpose.

There are several ways we might write an error check:

```
call applesauce || goto :EOF
```

```
call applesauce || exit /b
```

```
call applesauce || exit /b %ERRORLEVEL%
```

```
setlocal enabledelayedexpansion
call applesauce || exit /b %ERRORLEVEL%
```

```
call applesauce || goto :ERROR

...

    :ERROR
exit /b %ERRORLEVEL%  
```

Each one of these seems reasonable but misbehaves in at least one important case.
Getting this right is really hard!

## Unit Testing

Typically in programming we use unit tests to verify one piece of code without bringing in all the headache of executing the rest of the program.
To do that we need to isolate the code under test, for example by putting it in its own function. Can we do the same in Batch?

While Batch has functions:

```bat
    :APPLESAUCE
echo hello from applesauce
goto :EOF
```

But it's hard to call that Batch function in isolation.
Or at least we'd need to write a bunch of infrastructure in batch to make it possible.
That quickly gets use neck-deep in hazards.

We could isolate the code into a new file, a separate batch script.
That invite more hazards as well, and we'd quickly end up with a lot of small batch files to maintain together.

# What about Bash?

# What about PowerShell

## Direct execution

From a Windows CMD prompt if you execute a batch script, it will just run:

```
C:\>applesauce.cmd
hello from applesauce!
```

You can't do the same with a PowerShell script.
Running `applesauce.ps1` will open the file in Notepad.
This is a security feature: there was a time when attackers would send batch files as email attachments hoping people would double-click them and get owned.
(This is made worse by the Windows feature of hiding extensions of known file types, so the attached file might be called `pamela-anderson.gif.bat` but just look like `pamela-anderson.gif` in the UI.

Furthermore, PowerShell's execution policy is typically disabled by default, for same security reasons.

```
C:\>powershell ./applesauce.ps1
./applesauce.ps1 : File C:\applesauce.ps1 cannot be loaded because running scripts is disabled on this system.
```

We could tell people "enable execution policy and then..." but then we miss our "it just works goal".

# What about a binary executable?


# What problem are we really trying to solve here?

## Build-and-test

I think that for every source repo in the world, you should be able to clone it onto a clean machine and run the `build-and-test` script in the root and it should just work.

And some related goals:

- Two repos on the same machine should not interfere with each other (isolation)
- If we sync back in the repo, `build-and-test` should still work (version pinning)
- The same should apply to another other tool/script in the repo (DevEx)

Given how difficult it is to write batch files correctly, and how difficult it is to detect when we have made a mistake, I want to write as little Batch as possible. 
If I need to do anything more than the most trivial operation, I'd wrather write it in some other language and only use Batch as a bootstrapper.

# Solutions

## Linting

Suppose we write a linter for Batch, which detects the kinds of mistakes we are aware of.
Such tools already exist for most languages, including ShellCheck for Bash.
Why not do the same for Batch?

A linter needs to find a delicate balance, between accepting useful inputs on the one hand, and rejecting error-prone constructs on the other hand.
This is especially difficult for Batch, where there are so many reasonable-looking inputs that should be rejected, and lots of hazardous constructs that might be useful in some important context.

## Batch Generator

Suppose we write a generator for Batch. For example, to set a variable we might write this C#:

```C#
SetVariable("X", "Y");
```

Which would generate:

```
set "X=Y"
```

A generator is attractive for several reasons:

- we can pick a limited set of primitives that we can confidently support correctly
- we can implement lint-like checks at generation time without the burden of parsing arbitrary batch inputs
- we can isolate for testing by extracing a C# function and then testing what that function generates

## Package manager

When I look at the kinds of batch files I actually need to write to bootstrap into some better language, I see a set of common patterns like:

- Download a tool and put it on the path
- Download an archive, extract it, and put it on the path
- Download an installer and execute it
- Set environment variables

[Cache by version]



