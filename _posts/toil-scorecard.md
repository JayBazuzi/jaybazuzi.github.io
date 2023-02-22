---
layout: post
title: The Toil Scorecard
---

When a developer makes a code change to your software system, how long will it take for them to validate that their change works correctly and does not break existing behaviors? Is this something that only a seasoned expert can do, or can a new team member do it as well? What about rotating credentials or ensuring that 3rd-party dependencies are up to date? How do we observe that production is healthy and operating correctly? The Toil Scorecard gives teams a way to measure these costs and make toil visible to the organization.

# Measure time and decisions

Measure both how long it takes to do these activities and how many decisions are required (because executive function is a limited resource for everyone). It's OK to estimate to a single digit of precision.

# Grade the maturity

We can also give a grade to each activity:

- A: Fully automated; a human does not need to be involved.
- B: Partially automated, but a human needs to know how to wield or interpret the automation.
- C: The process is thorougly documented in a playbook that a human can follow.
- D: Someone on the team knows how to do this.
- F: No one in the team knows how to do this.

There is no partial credit. For example, automated tests cover most of the system, but for certain types of code changes we must manually test certain things, then the grade cannot be `B`.

# The Scorecard

Assemble the answers to these questions into a scorecard, which might look like:

| Area          | Score | Engineer Minutes per Month | Engineer Decisions per Month |
|---------------|-------|----------------------------|------------------------------|
| Validation    | B     | 600                        | 100                          |
| Release       | C     | 700                        | 200                          |
| Maintenance   | D     | 800                        | 300                          |
| Dependencies  | D     | 900                        | 400                          |
| Observability | B     | 1000                       | 500                          |

# How to use it.

Build a scorecard repo/project/service/system/tool/offering your team owns. Use this to see where your capacity is going and where to invest to reduce toil.

Collect the scorecards into a report that the boss's boss can peruse so they can support those investments. They'll probably want this for all their teams.

When thinking about the design of the next feature, consider how it will impact each of these metrics.

