---
layout: post
title: Why Refactor?
---

What is refactoring good for? What goals can be met with refactoring?

As I practice refactoring in my work, I keep learning new answers to these questions, which I'll summarize here:

* Refactor to make code readable, then read it. (Read-by-Refactoring)

Make code trustworthy. Fix overlong functions that overwhelm comprehension, without introducing the "now I have to look in two places" problem. 

* Refactor to make code testable, then test it. (Acceptance Microtests / Test-as-Spec / Decoupling / Cohesion / DRY)

We want tests that are super fast, completely reliable, and only fail when the subject code is broken.

We want tests that give us confidence that the system works, that verify that matter to users, that don't break when changing implementation details, and are written in terms that make sense to domain experts.

Most people think that only unit tests can match the first description. Most people thing that only acceptance tests can match the second description. They're both right, which is why Acceptance Microtests are ideal: they have all of the above characteristics.


* Refactor to make code changeable, then change it. (Decoupling / DRY)

One way to add a feature to code is to understand the design of the system, then figure out how to make your new feature fit, then code it up.

Another way is to ask "what design would make this feature easy to implement?", then refactor in that direction, then easily add the feature.

* Refactor to make code independent, then work on it independently. (Decoupling / Modularity / Scale)

Many large projects suffer from "scale" problems like slow builds, shotgun surgery, code review latency, too much or too little code ownership. SAFe has become popular in this context as a way to coordinate the work of large, multi-team projects, but another option is to make it possible for teams to work independently.

Good modules don't depend on / aren't dependend on by other code in the system. A team can build a new feature by building a new module, without needing to edit other code in the system or wait for another team to complete a subcomponent. Teams don't have to coordinate their work. A team doesn't have to worry about guarding their code against modifications by other people that don't know the consequences of their changes. Good modules can be owned entirely by one team, without interfering with other teams' work.

All of the above applies to equally to both modules and microservices. Good microservices can be owned, deployed, monitored, maintained entirely within a team.

* After a defect refactor to address the hazard. (Safeguarding / #BugsZero)

Skill, careful, concientious, well-intentioned developers still write defects. That's because our code is unsafe. 

But it doesn't have to be that way. We can make our code safe to work in.

Every defect (and every near miss) indicates a hazard. Use the defect as a learning opportunity to make the system safer.

* If we can't slice a User Story we like to less than 1/4 of a sprint, refactor to address the friction that stands in the way.
