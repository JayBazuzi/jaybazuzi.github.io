# Hermetic Python

Suppose we have some Python that depends on some 3rd-party libraries. How do ensure that those 3rd-party dependencies are stable over time, so we can get consistent results, in accordance with good engineering practice?

As I learned Python I was surprised at how haphazard this tends to be in the Python world, and it took a while to discover what I describe here. It wouldn't surprise me if someone finds a hole that allows some uncontrolled variation I am not aware of, or an easier way to do this - if you do, please let me know!

It's common to install Python system-wide but that exposes you to version mismatches (two Python projects that require different Python versions) and corruption (something installs a package in the system Python and now everyone gets it). Avoid this.

## Python itself

To the best of my knowledge there is no established way to declare the Python you require, except to imperatively check the Python version, e.g.:

```
assert sys.version_info == (3, 12, 0)
```

or

```
assert sys.version_info >= (3, 12)
```

This isn't satisfying. If you run with the wrong Python it fails but doesn't help you fix the problem. If you have multiple `.py` files in your context, you'll need to update all of them at once.

(Side rant: why the heck is `sys.version` a `str` but `sys.version_info` is a tuple? That distinction is too arbitrary.)

### Bootstrapping wrapper

In one project I have worked on, we had a "Python bootstrapper" which we would execute via an entry point script. We'd put tool "Foo" in `_foo.py` and then make a script to run it:

`foo`:
```Bash
#!/bin/bash
path/to/bootstrap_python_and_run.sh ${0%/*}/_${0##*/}.py "$@"
```

`foo.cmd`:
```Batch
@call path\to\bootstrap_python_and_run.cmd %~dp0\_%~n0.py %*
```

(This scripts are meant to run a same-named-but-`_`-prefixed script in the same directory. They aren't tested and may have a bug. Sorry.)

I'll leave it up to you to figure out how to write `bootstrap_python_and_run` because I have no idea how to do that.

## Virtual Environment

Don't ever* install a package into the installed Python, because that affects all users of that Python in difficult-to-control ways. Only ever install in to a virtual environment.

The steps to set up a virtual environment are _just_ complicated enough to be annoying:

```Bash
#!/bin/bash
set -euo pipefail
python -m venv .venv --system-site-packages
source .venv/path/to/activate
python -m pip install ...
```

```Batch
@setlocal
@echo off
call python -m venv .venv --system-site-packages || goto :EOF
call .venv\path\to\activate.cmd || goto :EOF
call python -m pip install ... || goto :EOF
```

(Details of `pip install` can be found below...)

Batch files are _incredibly_ difficult to get right, especially around error handling. It can be super subtle, so if you think you got it right, I'm here to tell you that it's probably wrong. So I'm always looking for a way to write less Batch. Bash and PowerShell are better but still hazardous.

I don't like the redundancy of maintaining both Bash and Batch implementations of this process. It's rare to find a team that is highly skilled at both, which just invites more subtle bugs.

Also, destroying the venv when it is out of date or corrupted is more headache but is missing from my above script snippets. Adding that makes it more complicated, right where I really don't want complexity.

### Tox and Nox

Tox says it "aims to automate and standardize testing in Python" and Nox says it "automates testing in multiple Python environments", but I see them as being far more powerful and important: they automate the management of virtual environments, not just for testing. With all the above complexity in mind, I much prefer handing over virtual environment responsibilities to Tox or Nox. Let me know if you need tips on how to set up Tox/Nox.

* I said before to never install packages into the installed Python, but Tox/Nox is my one exception. The Python bootstrapper can/should run `pip install tox/nox===SOMEVERSION` so it's available for everyone and stays consistent.

## Pip constraints

To ensure that we always install the exact versions of all our dependencies:

1. Create `requirements.txt` containing the primary dependencies. Version constraints are allowed but not required.
    ```
    SomeProject
    SomeOtherProject>=1,<2
    ```
1. Create an empty `constraints.txt` file.
1. In the venv setup script do `pip install --requirement requirements.txt --constraint constraints.txt --no-warn-script-location --disable-pip-version-check`
1. Run the venv setup script
1. `pip freeze > constraints.txt`

From now on, when you create the venv you should get the same complete set of dependencies. (On one project we found that we got slightly different deps on different platforms, so we split out `constraints-windows.txt`, etc.)

For bonus stability, modify your CI process to check that `pip freeze` exactly matches the contents of `constraints.txt`, to ensure that everything is correct/complete/stable as expected.

## Upgrading dependencies

To upgrade a dependency, remove entries from `constraints.txt` (remove all lines to upgrade all deps) and again run `pip install`/`pip freeze` as described above.

Dependabot does this for me automatically and I love it. Recommended!
