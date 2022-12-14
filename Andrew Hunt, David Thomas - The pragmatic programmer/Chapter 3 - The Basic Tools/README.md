# Chapter 3 - The Basic Tools

## Plain Text
Always be on the lookout for better ways of doing things.

__Keep Knowledge in Plain Text__

The power of text:
- **Insurance against obsolescence**. Human-readable forms of data, and self-describing data,  will outlive all other forms of data and the applications that created them.
- **Leverage**. Virtually every tool on the computing universe can operate on plain text.
- **Easier testing**. Simple matter to add, update, or modify the test data without
having to create any special tools to do so.

## Shell Games
For a programmer manipulating files of text, that workbench is the command shell. The command line is better suited when you want to quickly combine a couple of commands to perform a query or some other task.

**Invest some energy in becoming familiar with your shell** and things will soon start falling into place. Play around with your command shell, and you’ll be surprised at how much more productive it makes you.

Use Cygwin([https://www.cygwin.com]) to reach Unix-like utilities and functions.

## Power Editing
It is better to know one editor very well. If you use a single editor (or set of  eybindings) across all text editing activities, you don’t have to stop and think to  accomplish text manipulation: the necessary keystrokes will be a reflex.

Editor's feeatures:
- Configurable.
- Extensible.
- Programmable.

## Source Code Control
With a properly configured source code control system, you can always go back to a previous version of your software. You can manage branches in the development tree. SCCS may keep files in the central repository - a great candidate for archiving.

**Always Use Source Code Control**

One tremendous hidden feature of using SCCS - you can have product builds that are __automatic__ and __repeatable__. There're no manual procedures, and you won't need developers remembering to copy code into some special build area.

## Debugging

**Fix the Problem, Not the Blame**

Always try to discover the root cause of a problem, not just this particular appearance of it.

Two key points:
- You need to interview the user who reported the bug in order to gather more data than you were initially given.
- You must brutally test both boundary conditions and realistic end-user usage patterns.

The best way to start fixing a bug is to make it __reproducible__.

Visualize debugging data if you can.

**Rubber ducking**. A very simple, but particularly useful technique for finding the cause of a problem is simply to explain it to someone else.

**"select" Isn't Broken**

**Don't Assume It - Prove It**

When you fix the bug think, are there any other places in the code that may be susceptible to this same bug?

If it took a long time to fix this bug, ask yourself why. Is there anything you can do to make fixing this bug easier the next time around?

### Debugging Checklist
- Is the problem being reported a direct result of the underlying bug, or merely a symptom?
- Is the bug really in the compiler? Is it in the OS? Or is it in your code?
- If you explained this problem in detail to a coworker, what would you say?
- If the suspect code passes its unit tests, are the tests complete enough? What happens if you run the unit test with this data?
- Do the conditions that caused this bug exist anywhere else in the
system?

## Text Manipulation
We need to perform some transformation not readily handled by the basic tool set. We need a general purpose text manipulation tool.

**Learn a Text Manipulation Language**

A couple of applications to show the wide-range applicability of text manipulation languages:
- Database schema maintenance.
- Java property access.
- Test data generation. Knit together and convert tens of thousands of records of test data.
- Book writing. Check whether code is workable.
- Generation Web documentation. Produce HTML from code.

## Code Generators
A programmer can build a code generator. Once built, it can be used throughout the life of the project at virtually no cost.

**Write Code That Writes Code**

### Passive Code Generators
Once the result is produced, it becomes full-fledged source file in the project.
Passive code generators have many uses:
1. Creating new source files. Templates, source code control directives, copyright notices, standard comment blocks etc.
2. Perfoming one-off conversions among programming languages.
3. Producing lookup tables and other resources that are expensive to compute at runtime.

### Active Code Generators
Whenever you find yourself trying to get two **disparate environments** to work together, you should consider using active code generators.

Keep the input format simple, and the code generator becomes simple.