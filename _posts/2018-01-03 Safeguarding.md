---
layout: post
title: What is Safeguarding? (A definition)
---

At Tableau we use the term "Safeguarding" to describe a particular way of reducing future defects by learning from past defects. Arlo described it in [a parable](http://arlobelshee.com/improving-testing-is-not-safe-a-parable/). Here's my current concrete understanding:

Safeguard after you address a problem.
===

If production goes down, bring it up first. If you find a bug, fix it first. 

Safeguard as soon as the fire is out / the bleeding has stopped. Safeguard before returning to other, non-emergency tasks.

Safeguard the genus.
===

If you fix a bug, the software is better for your users, but it is not safer for developers to work in. Even adding a test for the exact defect you're fixing is not good enough; we'll never cover them all that way. Look deeper. Learn something about the underlying causes. 

What made it possible to create this problem? What factors led to this happening? Was something confusing to us? Have we created this kind of problem before, and what is the commonality? 

Safeguarding is quick and lightweight.
===

Now that you've identified a deep underlying cause for a broad genus of problems, don't fix the whole genus at once. Find one thing you can do to make things a little bit better. If you can reduce the size of the genus by 15%, that's about right. 

Look for something you can do in about an hour. This makes it easy to justify the cost. You'll capture the value of your efforts quickly. If your idea doesn't pan out, you haven't lost much. If you get interrupted during the 3rd hour, you'll still have two safeguards completed.

For serious problems, Safeguard repeatedly.
====

If production was down for a day, it's worth spending more than an hour of your time to make sure that doesn't happen again. But don't spend a week rewriting a subsystem. Instead do a series of quick and light safeguards, even if you do them for a whole week. 

Some examples
====

| Idea | Is this safeguarding? |
|------|-----------------------|
| Add a test when fixing a bug. | No. |
| Find a second instance of a bug you're fixing. Merge duplicate code. Add a test. | Yes. |
| Convert integration tests to unit tests. | Yes. |
| Add a story to the backlog to refactor some gnarly code. | No. | 
| Factor 1 responsibility out of some gnarly code. | Yes. |
| Around the code where you're fixing a bug, rename 1 local variable. | Yes. |
| Add a static analysis check for a construct that led to a bug. | Yes. |
| Tell other developers not to make the same kind of mistake. | No. |

Is this the Camp Site Rule?
====

Safeguarding is closely related to the Camp Site Rule, aka the Scouting Rule, "Always leave the campground cleaner than you found it.", but applies to a more specific context (when addressing a problem) and is a more constrained action (15% reduction of the genus).

Is this Refactoring?
====

Some safeguarding is refactoring. Some is testing. Some is tooling. 

It's not education, which puts a burden on developers. 
