---
layout: post
title: How to make Source Server work with Git
published: True
excerpt_separator: <!--end-of-excerpt-->
---

Git doesn't plug in easily with Source Server. How can we make it work?

(Part 4 of a series on Windows debugging.)

<!--end-of-excerpt-->

Configuring Source Server with your typical centrialized version control cystem is straightforward, as [discussed last time](https://jay.bazuzi.com/Debugging-3-finding-source-code/). You just need a way to print out a file from the central server, at a specified revision. But there is no consistently-reliable way to do this in Git.

Partly this is because DVCS is just different than a centralized VCS. There is no "central" server you can ask for that revision. By convention, you may treat one repo as "central" (e.g. the one in your GitHub organization) but that's a convention, not a feature of Git.

Also, Git doesn't have a built-in way to print out the contents of an arbitrary file from a remote repo. `git show` only works in a local repo, not remotely.

I'm aware of several possible approaches in Git, each of which has some unfortunate limitations.

# wget/curl

You could fetch the file directly from the central repo with HTTP(S), e.g.:

```
wget https://raw.githubusercontent.com/JayBazuzi/jaybazuzi.github.io/a3124c74199a06d21cdb29fb8eb88d46813f3ac4/README.md
```

This assumes you have permissions to access these files via HTTP(S). That wasn't true at one company that used GitHub Enterprise Cloud: the developer's machines were on a separate, untrusted network, and could access the repo via SSH but not the GitHub web API.

Also, until recently Windows didn't have a built-in `wget` or `curl` command. (curl was added to Windows 10 not too long ago.) Maybe your enterprise maybe you can guarantee that one is installed and on the path?

PowerShell has `Invoke-WebRequest` which looks handy. You need to use `-OutFile` not redirection (`>`). Also, PowerShell might be disabled per your enterprise's security policy. Ok, let's not use PowerShell.

Another option is `bitsadmin`:

```
bitsadmin /transfer "source sever is fun!" https://raw.githubusercontent.com/JayBazuzi/jaybazuzi.github.io/a3124c74199a06d21cdb29fb8eb88d46813f3ac4/README.md C:\path\to\foo.cpp
```

Hint: bitsadmin requires the output path is fully qualified.

Uggh, that was quite a tangent. I thought were were talking about debugging?

# SSH

Depending on where your central repo is hosted, perhaps you can SSH to the server to access the file:

```
ssh me@otherhost "cd repo && git show ..."
```

This won't work on GitHub, though - GitHub doesn't allow interactive SSH access.

I learned this approach from [this StackOverflow question](https://stackoverflow.com/questions/1178389/browse-and-display-files-in-a-git-repo-without-cloning).

# Local Clone

The source server command could use `git clone {URI} {SOMEWHERE} && cd {SOMEWHERE} && git show {REVISION}:path/to/file.cpp`. 

This depends on cloning permissions - if you are set up with an SSH key but the source server command uses HTTPS (or vice versa), you might be out of luck.

It also assumes a small enough repo that `git clone` is quick - a large repo may take too long to clone to make this viable. You can speed things up by replacing `git clone` with `git init & git fetch` (skipping the checkout). Also, maybe only fetch the revision you're looking for.

Where will this repo live? Will you end up with multiple clones this way? How will you clean them up? 
Will the approach you implement today still be managable in the future?
All of this gets more complicated as the number of machines, developers, and years grow.

# Use an existing clone

People doing the debugging are probably developers, and developers have probably already cloned the repo on their computer already. Can we use that?

Yes, but we need to find the repo somehow. Maybe you already a well-known location for the repo by your organization's convention, but I prefer a well-known environment variable. If you make the source server command something like `cd %MYPROGRAM_REPO_LOCATION_FOR_SOURCE_SERVER% & git fetch & git show ...`, then you can tell developers to run `setx MYPROGRAM_REPO_LOCATION_FOR_SOURCE_SERVER C:\path\to\repo` before launching Visual Studio. 

Note the `git fetch` to ensure that the new revisions are available, in case the developer hasn't done a `fetch` in this repo for a while and the needed revision is not yet present.

# A final rant

I sure wish Git would add a command for this purpose, like `show-remote` or something. It could be used like this:

```
git show-remote <URI> a3124c74199a06d21cdb29fb8eb88d46813f3ac4:README.md
```

That would make this problem easier to solve.
