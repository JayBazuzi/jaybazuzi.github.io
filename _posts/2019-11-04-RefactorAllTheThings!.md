---
layout: post
title: Refactor all the Things!
---

As I've practiced refactoring over the years, I continue to find new ways to solve problems better with refactoring. This is why refactoring is such a big deal to me.

What are some of the problems we can solve with refactoring? This is hard to talk about because many of these problems aren't generally recognized as solvable / we don't know what the solutions would look like, so the idea that we would refactor to fix them is a hard to comprehend. Each of these is a whole 'nother article / book / course, but for now I'll summarize.

## Reading

Before you can work effectively in any code, you must read it. Legacy code is generally difficult to read. Identifier names are misleading. Functions are overlong.

Rename to make code trustworthy to readers. Break up overlong functions that overwhelm comprehension, without introducing the "now I have to look in two places" problem. Merge fragmented code.

These kinds of refactorings are easy, cheap, safe, and reversable. These tiny investments pay back immediately by making you more productive right away. Readability is a prerequisite to all the other work you might want to do in code. You should totally be doing this right now.

Arlo Belshee calls this [Read-by-Refactoring](https://insightloop.digdeeproots.com/dl/train-the-trainer-rbr-insight-loop.pdf).

## Testing

We want tests that are super fast and completely reliable. When a bug is introduced, it should only fail one test, not a bunch of unrelated tests. The problem should be immediately obvious from the failure.

We want tests that give us confidence that the whole system works, not just the parts. Tests should verify what actually matters to users. Tests shouldn't break when we only change implementation details. They should be written in a way that makes sense to domain experts.

Most people think that only unit tests can meet the former, and that only acceptance tests can meet the latter. They're right, which is why Acceptance Microtests are ideal - they have all of these characteristics. See https://twitter.com/jaybazuzi/status/1192090776676601856

Acceptance Microtests are only possible in well-factored code, so you'll need to refactor your way there. Arlo Belshee calls this Test-by-Refactoring.

## Easy to change

One way to add a feature to code is to understand the design of the system, then figure out how to make your new feature fit into that design.

Another way is to ask "what design would make this feature easy to implement?", then refactor in that direction, then easily add the feature.

When extending the behavior in a new direction, we should not need to worry about or update all the existing features.

## Safe

Skilled, careful, conscientious, well-intentioned developers still write defects. That's because our code is unsafe.

But it doesn't have to be that way. We can make our code safe to work in.

Every defect (and every near miss) indicates a hazard. Use the defect as a learning opportunity to make the system safer.

Over time the defect rate drops steadily until you go from measuring "number of defects this week" to "number of features shipped since last defect".

See:
- [Safeguarding – Culture is a Process, not a Single Change](http://arlobelshee.com/safeguarding-culture-is-a-process/)
- [Safeguarding](http://jay.bazuzi.com/Safeguarding/)
- [Safeguarding: A step-by-step guide](https://llewellynfalco.blogspot.com/2018/12/safeguarding-step-by-step-guide.html)

## Scaling and independence

Many large projects suffer from "scale" problems like slow builds, shotgun surgery, code review latency, too much or too little code ownership.

Instead of adding more process/protocol/structure to manage the growing complexity, refactor to create good modules.

Good modules don't depend on / aren't depended on by other code in the system. A team can build a new feature by building a new module, without needing to edit other code in the system or wait for another team to complete a subcomponent. A team doesn't have to worry about guarding their code against modifications by other people that don't know the consequences of their changes. Good modules can be owned entirely by one team, without interfering with other teams' work. Teams don't have to coordinate their work. 

[Llewellyn's Open/Closed strategy can be useful here.](http://jay.bazuzi.com/LOSOCS/)

All of the above applies to equally to both modules and microservices. Good microservices can be developed, deployed, monitored, and maintained entirely within a team. See [Thoughts On Microservices](http://jay.bazuzi.com/Thoughts-on-Microservices/).

## User stories should fit in sprints

User Stories should be [INVEST](https://www.agilealliance.org/glossary/invest/).

If you have a Product Owner, they should be able to pick a few stories to work on next. If your user stories are too big to do that, you probably need thin vertical slicing.

But in some projects the problem isn't the size of the idea, it's the developer friction that makes even tiny ideas expensive to implement. If developers are spending most of their time reading inscrutable code, waiting for the build, rerunning flaky tests, etc., then slicing more thinly won't help.

If we can't slice a User Story to less than 1/4 of a sprint, the Product Owner allocates the next sprint for refactoring to address the friction that is slowing the team down. Note that this is PO-driven because a PO needs it to be able to do their job effectively.

# Refactoring is good

I feel like I need something here to tie everything together, but it's not coming to me. I like that refactoring is safer than editing, that my commit history is easier to understand, that I can integrate continuously, etc. Finding ways to apply refactoring to more problems means I get more of these benefits.

If you consistently use refactoring in your work, superficially it may look like you're "not getting any work done" most of the time. Have a conversation about this early to help people understand what to expect.

