---
layout: post
title: The Toil Scorecard
---

"Toil" is the work that takes time and energy human capacity but could be largely automated. "Technical Debt" is generally hard to measure, but Toil is measurable. It includes activities such as validating a code change, rotating credentials, and updating 3rd-party dependencies. The Toil Scorecard gives teams a way to measure these costs and make toil visible to the organization.

For each area we give a qualitative score, a quantitative measurement of time spent, and a quantiative measurement of decision-making.

# Grade the automation maturity

| Score | Description                                                                           |
|:-----:|---------------------------------------------------------------------------------------|
| A     | Fully automated; a human does not need to be involved.                                |
| B     | Automated but a human needs to know how to wield or interpret the automation.         |
| C     | The process is thorougly documented in a playbook that a non-expert human can follow. |
| D     | Someone on the team knows how to do this.                                             |
| F     | No one in the team knows how to do this.                                              |

Give a separate score for each area.

There is no partial credit within an area. For example, if some parts are written down in a playbook but other parts are in the expert's head, then score a `D`.

# Measure time and decisions

Measure both **how long** it takes to do these activities and how many **decisions** are required, because executive function is a limited resource for everyone. 

Aggregate per month. In the Toil Scorecard we don't need to differentiate between "fast + frequent" vs. "slow + seldom".

A single digit of precision is plenty. Even order-of-magnitude is good enough, especially at the beginning.

# The Areas

| |
| - | 
| **Validate** (test) an arbitrary change |
| **Release** (ship) to production |
| **Maintain** the in-production system |
| Keep up to date with **Dependencies** |
| **Observe** the health of the system |

## Validation

Test that an *arbitrary* code change works correctly and does not break existing behaviors, without relying on a mental model of the system.

### Examples

| Score | Description                                                                                                         |
|:-----:|---------------------------------------------------------------------------------------------------------------------|
| A     | Automated tests cover everything and automatically run in CI with every change.                                     |
| B     | A human executes the automated tests and/or interprets the results.                                                 |
| C     | There's a document that says "if you change the Payments module, you must manually verify that payments still work" |
| D     | The dev who wrote the Payments module knows how to test it and watches for PRs that touch that area.                |
| F     | We don't know that the Payments module requires special treatment, because the dev who wrote Payments has moved on. |

If you have specialized testers, sysops, devops, contractors, include their activity in this assessment.

## Release

Ship to production. Include required paperwork and sign-offs. Remember to account for special cases like "this release includes a database migration".

If additional validation is necessary to release ("hardening sprint" or "manual test pass"), include that in the Validation cost.

### Examples

| Score | Description                                                                                                                       |
|:-----:|-----------------------------------------------------------------------------------------------------------------------------------|
| A     | Every time a PR is merged to `main` it is automatically deployed to production.                                                   |
| B | (TBD) |
| C     | We have a release playbook. Both the experienced team members and the person that joined yesterday can follow it in the same way. |
| D     | Each person "knows" how to release but they each do it differently.                                                               |
| F | (TBD) |

## Maintenance

Details vary widely between systems. A good question to start with is "If the humans went away, how long would the system keep functioning?"

### Examples

| Score | Description                                                                |
|:-----:|----------------------------------------------------------------------------|
| A     | SSL certificates are automatically updated well before they expire.        |
| B | (TBD) |
| C | (TBD) |
| D     | Every Monday you clear out the old logs so we don't run out of disk space. |
| F | (TBD) |

## Dependencies

Are we using the latest version of every 3rd-party dependency?
How do we know when a new version becomes available?
How do we know if it contains an urgent security fix?
What is the process for updating?

Most systems have a few extra dependencies hiding in the crevices. Is your CI build using the latest version of the OS? Is your build system using the latest version of Maven/CMake/Bazel/Gradle? Be sure to include these in your assesment.

### Examples

| Score | Description                                                   |
|:-----:|---------------------------------------------------------------|
| A     | dependabot sends an automerge PR for every updated dependency |
| B     | dependabot sends a PR but a human reviews and approves        |
| C | (TBD) |
| D | (TBD) |
| F     | we use Python 2.7 to automate the release process             |

## Observability

Details vary widely between system. A good question to start with is "How do we know that production is healthy?"

If you have dedicated SysOps or DevOps folks, include their activity in this assessment.

### Examples

| Score  | Description                                                                                                                    |
|:-:|-------------------------------------------------------------------------|
| A | (TBD) |
| B | We get paged when things go wrong.                                      |
| C | (TBD) |
| D | Ops has learned to watch for out-of-memory errors and reboot the server.|
| F | We find out that the site is down when our users complain.              |


# The Scorecard

Assemble the answers to these questions into a scorecard, which might look like:

| Area          | Score | Engineer Minutes per Month | Engineer Decisions per Month |
|---------------|:-----:|:--------------------------:|:----------------------------:|
| Validation    | B     | 600                        | 100                          |
| Release       | C     | 700                        | 200                          |
| Maintenance   | D     | 800                        | 300                          |
| Dependencies  | D     | 900                        | 400                          |
| Observability | B     | 1000                       | 500                          |

# How to use it.

* Build a scorecard for each repo/project/service/system/tool/offering your team owns. Look for hot spots - where a lot of time and decision-making capacity is consumed - to decide where to invest to reduce toil.

* Collect all the scorecards into a report that the boss's boss can peruse so they can support those investments. They'll probably want this for all their teams.

* When thinking about the design of the next feature, consider how it will impact each of these metrics.

* Use Arlo's Belshee's Automation-as-a-Process ([video](https://www.youtube.com/watch?v=ydq-KjGDRJg), [article](https://digdeeproots.substack.com/p/when-should-i-automate)) to quickly get to `B`.

* We all have blind spots. We may think the tests are complete but then a defect slips through and shows us what we didn't account for. In these cases, lower the score appropriately, then do [Safeguarding](/Safeguarding) to address the hazard.

* Extend this system to measure what is important in your context.

# What's not covered

Lots! For example:

* What is the wall-clock latency in these areas?
* How important is it to the business that this system has high quality and uptime.?
* Time spent reading and understanding code. (This is probably huge!)
* Time spent doing code reviews or revising changes in response to review feedback.
* Time spent fixing defects.
* Time spent building the wrong feature because we didn't get feedback earlier.

These are important concerns, but I'm deliberately not including them in the Toil Scorecard. Maybe they'll show up in some other diagnostic system?
