Tests are neither neither necessary nor sufficient to refactor safely.

Here's a tale as old as TDD:

We think this unit testing thing is a great idea! We wish we could do it on all of our code. Unfortunately this is legacy code, so there's no good way to get tests around many parts of the system. We'd like to refactor to make our code more testable, but the last time we tried that, we introduced some bugs and now the business thinks refactoring is dangerous and doesn't want us to do it any more. 

We can't refactor safely without tests; we can't test without refactoring.

I hope one day we can rewrite from scratch so we can do it right, test-first from the beginning. Until then, we'll just have to use integration tests and manual tests to 
