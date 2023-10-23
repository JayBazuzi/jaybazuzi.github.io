What does it mean for refactoring to be "safe"? If one approach is more or less safe than another, how can we compare them? How safe is safe enough?

- What is the risk?
    - Stakeholders overreact to refactoring-introduced defects.
    - Programmers underestimate stakeholder sensitivity to refactoring-introduced defects.
    - As much as we abhor refactoring-introduced defects, the consequences of _not_ refactoring are even greater (but harder to recognize).
- Do we have tests?
    - Strict TDD doesn't produce untested features.
    - Retrofitting tests leaves untested features.
    - Hyrum's Law means that you probably have test gaps anyway.
- Is it automated?
    - Automation probably has flaws.
    - VC# flaws are fringe.
    - PyCharm flaws are glaring.
    - Compiled, type-safe languages have a huge advantage.
    - Build system integration (MSBuild, `compile_commands.json`) have a huge advantage.
    - "pretty good" automation can be mitigated.
        - Make a pre-flight checklist.
        - Report a bug to the author.
        - Bugs stay fixed.
- Human-executed recipe
    - Humans are inconsistent.
    - Humans can lean on the compiler to do the heavy lifting.
    - C++ Explicit Capture is hugely helpful; most languages don't have this.
    - With careful analysis and testing we can declare a recipe "vetted", but still may be flawed.
- Built in to the language
    - For example https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/expressions#122032-query-expressions-with-continuations
        > A query expression with a continuation following its query body
        > ```
        > from «x1» in «e1» «b1» into «x2» «b2»
        > ```
        > 
        > is translated into
        > ```
        > from «x2» in ( from «x1» in «e1» «b1» ) «b2»
        > ```
    - Wish more languages had transformations in the language.


<!--

- Do we have great tests? This isn't perfect, because tests are almost always incomplete (see [Hyrum's Law](https://www.hyrumslaw.com/) but it's better than not having tests.
- Do we have an automated tool? Tool correctness varies _very_ widely, but the good news is that tools are consistent. That means we can identify, report, and mitigate their limitations.
- If it's a manual process, is it written down? 


## Do we have Tests?

A common refrain is that we must have tests in order to refactor safely. I love tests, but I also know that test suites are generally incomplete. Even well-tested systems aren't generally completely safe to refactor due to [Hyrum's Law](https://www.hyrumslaw.com/):

> With a sufficient number of users of an API,
> it does not matter what you promise in the contract:
> all observable behaviors of your system
> will be depended on by somebody.

That includes bugs that you don't know about but which someone has come to depend on - there's no way you'll have a test for a bug you don't know about!

## Proven Transformations

Programming languages are well-defined systems with rules (the language spec). According to those rules, there are certain transformations that are behavior-idempotent. These are the gold standard of refactoring safety.

Identifying/defining these transformations requires the same kind of rigor as writing the language itself. I wish that language designers would include refactorings in their specifications.

Here's one example -- C#'s query expressions are defined as such transformations: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/expressions#122032-query-expressions-with-continuations

> A query expression with a continuation following its query body
> ```
> from «x1» in «e1» «b1» into «x2» «b2»
> ```
> 
> is translated into
> ```
> from «x2» in ( from «x1» in «e1» «b1» ) «b2»
> ```

## A Vetted Process

Through careful study we can define a process for transforming code and then people to critique it and use it in their work to find the limitations and then we'll call it "vetted".

The process might be manual, like this [C++ Extract Function Recipe](https://jay.bazuzi.com/Safely-extract-a-method-in-any-C++-code/).

It might be automated, like the Rename for C# in Visual Studio.

Just being automated doesn't make your refactoring safe. For example, PyCharm gets even simple renames wrong. But automation has advantages:
- It will behave consistently (humans are inconsistent).
- When we identify a limitation, we can write it down.
- We can file a bug report against a tool, and when it gets fixed it will stay fixed. (This doesn't work on humans.)

Writing down the limitations (and their workarounds) makes a kind of hybrid process, where humans do part of the work and automation does part of the work.

## Just editing code

Programmers edit code all the time. What is the different between refactoring by editing code manually, and other kinds of development work? Why isn't Extract Method just "copy and paste"? Why isn't Rename just "find and replace"? 

It can be fine to just edit, specially for new code that is not in use yet. But for code that someone is already counting on, stakeholder's risk tolerance is much lower for refactoring than for feature/bugfix changes. That's because stakeholders clearly see the value in features and bugfixes, but the value in refactoring is harder for them to see. And just as stakeholders underestimate the importance of refactoring, programmers underestimate stakeholder's risk sensitivity to refactoring.

If we have comprehensive tests the risks go way down, but as I mentioned before - tests are not a panacea and don't eliminate all risks.

-->
