---
layout: post
title: Measuring 3PP Health
---

If a critcial security vulnerability is found in a library we depend on, how quickly can we find out, apply the fix, verify, and ship to production?

Pretty much every software project today depends on a myriad of 3rd-party libraries and programs.
While generally highly benefical, these can become a source of technical risk and threat to the health of the business if not managed well.

Furthermore, 3PP age is a pretty decent first-approximation of the technical health of the software system and the organziation that maintains it:
organizations that are able to stay on top of their 3PP are able to maintain high quality in their work, ship frequently, and respond to promptly to shifting conditions.

# A single measure: 3PP age

The first 3PP health measure is "3PP age, in days". For each 3PP we depend on, how long ago was it released?
Aggregate this by adding all the values of all the 3PP we depend on.

One organization I know tracks this in a spreadsheet, like:

| 3PP Name | Version | Age (days) |
| - | - | - |
| npm | 10.0.0 | `TODAY() - DATE("Aug 31, 2023")` |
| lodash | 4.0.0 | `TODAY() - DATE("Jan 12, 2016")` |
| TOTAL | | `SUM(C)` |

The nice thing about this measure is that it's easy to understand and explain.
Because we measure in a small unit (days) it looks quite large, which is appropriate.
"Our 3PP is 1.4 gajillion days out of date, and that is a business risk that we should address."

Note that if a 3PP we depend on is not updated for a long time, this measure will get worse even if we are consistently on the latest version.
That is also appropriate: an unmaintained dependency is a business risk for us.

## Other measures

This list is in rough priority order - start at the top, but you need not hit 100% success in each one before you move to the next one.

1. We know every direct dependency.
1. Every direct dependency is on an actively-supported version.
1. We know all our direct and transitive dependencies, and their exact versions.
1. We know if any of those dependencies contain a disclosed security vulnerability.
1. We know if there are available updates for any dependency.
1. We know we are on the latest patch (latest minor (latest major)).
1. We know the process for updating each 3PP
1. Producing a report of the above is fully automated

