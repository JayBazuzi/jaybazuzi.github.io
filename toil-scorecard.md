Title: The Toil Scorecard

When a developer makes a code change to your software system, how long will it take for them to validate that their change works correctly and does not break existing behaviors? Is this something that only a seasoned expert can do, or can a new team member do it as well? What about other activities like releasing to production and carrying out regular maintenance? The Toil Scorecard gives teams a way to measure these costs and make toil visible to the organization.

In addition to how long it takes to do these activities, we should also measure how many decisions are required (because executive function is a limited resource for everyone) and how often we need to do them.

We can also give a grade to each activity:

A: Fully automated; a human does not need to be involved.
B: Mostly automated, but a human needs to how to wield or interpret the automation.
C: The process is thorougly documented in a playbook that a human can follow.
D: An expert on the team knows how to do this.
F: No one in the team knows how to do this.

There is no partial credit. For example, automated tests cover most of the system, but for certain types of code changes we must manually test certain things, then the grade cannot be `B`.

What about the process of releasing to production? Carrying out routine maintenance, such as rotating credentials? Ensuring that 3rd-party dependencies are up to date? Observing that production is healthy and operating correctly? 

Put all of these together in a report, for each repo/project/service/system/tool/offering your team owns. Something like:



**FooBar service**

| Area | Score | Engineer Minutes per Month | Engineer Decisions per Month |
| - | - | - | - |
| Validation | B | 600 | 100 | 
| Release | D | 700 | 200 |
| Maintenance | 800 | 300 |
| Dependencies | 900 | 400 |
| Observability | 1000 | 500 |

