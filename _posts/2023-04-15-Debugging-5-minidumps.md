---
layout: post
title: Understanding minidump debugging
excerpt_separator: <!--end-of-excerpt-->
---

## Programs are just data in memory

Code is just data. Running programs are just data in memory. Debuggers just inspect that data. What if we take a snapshot?

(Part 5 of a series on Windows debugging.)

<!--end-of-excerpt-->

Consider again the code in [Part 1](https://jay.bazuzi.com/Debugging-1-debug-info/):

```
const auto message = "Hello World!";
std::cout << message << std::endl;
```

Here it is as viewed in the VS Debugger Disassembly window, with "Show Code Bytes" option enabled:

```
009F1934 C7 45 F8 30 7B 9F 00 mov         dword ptr [message],offset string "Hello World!\n" (09F7B30h)  
    printf(message);
009F193B 8B 45 F8             mov         eax,dword ptr [message]  
009F193E 50                   push        eax  
009F193F E8 8E F7 FF FF       call        _printf (09F10D2h)  
009F1944 83 C4 04             add         esp,4  
```

We can see the same data in the Memory window:

```
0x009F1934  c7 45 f8 30 7b 9f 00 8b 45 f8 50 e8 8e f7 ff ff 83 c4 04
```

See, it's just data!

## Memory dumps

Suppose we take a snapshot of all the memory of a program. We could save it to a file, move that file to another computer, point the debugger at it, and see all the same variables, callstacks, threads, etc. This is handy when the program crashes in production or in a test environment - take the snapshot so developers can inspect it later. (That's exactly what the "Watson" feature in Windows does.)

You don't even have to wait for a crash - you can save a minidump any time you notice a strange behavior in the program.

If you save the entire memory space of a program, it would be huge. Luckily and not all the data in memory is required for analysis. For example, we can elide the code itself, because that is already available in the `.exe` and `.dll` files we already have. When we attach the debugger to a minidump, the debugger can fill in the missing code from your local binaries.

## Symbol Server and memory dump debugging

Finding the right matching binaries can be a headache, just like finding the right matching PDB and source files. That's why symbol server works for binaries as well as debug info.

If you're collecting minidumps from your software's crashes (you should), you should be publishing both binaries and symbols from official builds to a symbol server.

Sadly, I can't find a good reference document on how to do this. Start with [this blog post](https://www.timdbg.com/posts/symbol-indexing/).

