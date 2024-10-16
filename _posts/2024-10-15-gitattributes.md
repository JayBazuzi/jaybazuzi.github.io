---
layout: post
title: The one true .gitattributes
---

I take it as a fundamental tenet of source control systems that I can put a file in now and get it out later, unchanged. Git breaks this rule: Git can convert line endings, which which regularly causes confusion and file corruption. Let's stop that from happening.

## The problem + background

- If you tell Git to convert line endings to UNIX style (`LF`), it will corrupt binary files that happen to contain a `0D0A` sequence.
- `.cmd` scripts _must_ use `CRLF` line endings (see https://www.dostips.com/forum/viewtopic.php?t=8988)
- it's super-confusing when a text file is checked out with one kind of line ending on one machine and another kind of line ending on another machine

Git's `.gitattributes` file defaults to treating every file as `text` which means:
- the file is eligble for line-endings conversion
- `git diff` should show the diffs (GitHub appears to respect this as well)

## The solution

Every repo should have a `.gitattributes` file containing:

```
* -text diff
```

this means:
- no files are eligble for line-ending conversion
- show diffs

In my opinion, `git init` should create this `.gitattributes` file, avoiding a lot of confusion and file corruption. For now, you have to do it yourself. You can automate it locally by placing such a file in a "Git template" directory and configuring Git like this:

```
git config --global init.templateDir '~/.git-template'
```

Also, you may want to disable diffs for any binary files you add by appending something like:

```
.png binary
```
