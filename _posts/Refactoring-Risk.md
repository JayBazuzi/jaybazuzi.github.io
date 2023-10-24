# Refactoring Risk

What risks exist around refactoring? How risk averse should we be?

## Introducing defects

We developers edit code all the time, and sometimes we introduce defects.
Why should refactoring be any different than feature or bugfix work?

If we introduce a defect while writing a feature, stakeholders may sigh and ask us to be more careful.
But if we introduce a defect while refactoring, stakeholders might say we should stop refactoring.
They'll never say we should stop writing new features.

This is a risk that developers may not fully recognize.
When I describe this to other developers, sometimes they say "that wouldn't happen here," and then it happens here.

Sometimes it's subtle.
Instead of "no more refactoring", they may say "no refactoring during the hardening sprint" or similar.

## Risk of not refactoring

Refactoring is _critically_ important for the long-term health of the business.
Without sufficient refactoring technical debt grows, velocity slows, and defect injection rates increase.
Yes, not refactoring will _increase_ the rate of defects over time.
In this way, restricting refactoring to reduce risk will backfire eventually.

The business absolutely needs us refactor, to keep the codebase viable and sustainable for the business's needs.

Most stakeholders (and many developers) don't fully recognize the business risk of insufficient refactoring.
I'm here to tell you it's a big deal.

## We were going to write that bug anyway

Bugs don't just arise out of nowhere.
They have causes.
Every bug indicates a hazard in the system - a hazard which makes it more likely that a well-intentioned developer would make a mistake.

If we introduce a bug while refactoring, there's a decent chance that we would have introduced a bug in the same way when we edit this area for a feature / bugfix.
The bug injection risk was always there, waiting to happen.
Attributing it to refactoring misses this detail.

## Hold refactoring to a higher standard

With this in mind, I argue that we developers must refactoring aggressively, but that we must hold our refactoring to a higher expectation of safety than feature / bugfix work.
