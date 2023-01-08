# Chapter 6 - While You Are Coding
Coding is not mechanical. There are decisions to be made every minute.

## Programming by Coincidence
We should avoid programming by coincidence — relying on luck and accidental successes—
in favor of __programming deliberately__.

Sometimes it can be pretty easy to confuse a happy coincidence with a purposeful plan. 

Why should you take the risk of messing with something that’s working?
- It may not really be working — it might just **look like it is**.
- The **boundary condition** you rely on may be just **an accident**. In different circumstances (a different screen resolution, perhaps), it might behave differently.
- **Undocumented behavior** may change with the next release of the library.
- Additional and **unnecessary calls** make your code slower.
- **Additional calls** also increase the risk of introducing new bugs of their own.

For routines you call, rely only on documented behavior. If you can’t, for whatever reason, then document your assumption well.

At all levels, people operate with many assumptions in mind — but these assumptions are rarely documented and are often in conflict between different developers.

**Don't Program by Coincidence**

## How to program Deliberately
- Always be aware of what you are doing.
- **Don't code blindfolded**. Attempting to build an application you don’t fully understand, or to use a technology you aren’t familiar with, is an invitation to be misled by coincidences.
- **Proceed from a plan**, whether that plan is in your head, on the back of a cocktail napkin, or on a wall-sized printout from a CASE tool.
- **Rely only on reliable things**. Don’t depend on accidents or assumptions. 
- **Document your assumptions**.
- **Don’t just test your code, but test your assumptions as well**. Don’t guess; actually try it.
- **Prioritize your effort**.
- **Don’t be a slave to history**. Don’t let existing code dictate future code.

So next time something seems to work, but you don’t know why, **make sure** it isn’t just a coincidence.

## Algorithm Speed
Another kind of estimating that Pragmatic Programmers use almost daily: estimating the resources that algorithms use—time, processor, memory, and so on.

The __O()__ notation is a mathematical way of dealing with approximations.

<p align="center">
  <img width="400" src="https://github.com/rsaitov/Books/blob/master/Andrew%20Hunt%2C%20David%20Thomas%20-%20The%20pragmatic%20programmer/Chapter%206%20-%20While%20You%20Are%20Coding/O-notation.png" />
</p>

**Test Your Estimates**

### Best Isn’t Always Best
You also need to be pragmatic about choosing appropriate algorithms—the fastest one is not always the best for the job. 

## Refactoring
Software is more like **gardening**. You plant many things in a garden according to an initial plan and conditions. Some thrive, others are destined to end up as compost. 

Rewriting, reworking, and re-architecting code is collectively known as **refactoring**.

Any number of things may cause code to qualify for refactoring:
- Duplication.
- Nonorthogonal design.
- Outdated knowledge.
- Perfomance.

**Refactor Early, Refactor Often**

Keep track of the things that need to be refactored. If you can’t refactor something immediately, make sure that it gets placed on the schedule.

Martin Fowler's tips:
1. Don't try to refactor and add functionality at the same time.
2. Make sure you have good tests before you begin refactoring. Run the tests as often as possible. That way you will know quickly if your changes have broken anything.
3. Take short, deliberate steps: move a field from one class to another, fuse two similar methods into a superclass. 

If it hurts now, but is going to hurt even more later, you might as well get it over with. Remember the lessons of Software Entropy: **don’t live with broken windows**.

## Code That's Easy to Test

We need to build testability into the software from the very beginning, and test each piece thoroughly before trying to wire them together.

A software **unit test** is code that exercises a module. Typically, the unit test will establish some kind of artificial environment, then invoke routines in the module being tested.

We like to think of unit testing as __testing against contract__. When you design a module, or even a single routine, you should design both its contract and the code to test that contract.

Providing unit tests isn’t enough. You **must run them**, and run them **often**.

**Test Your Software, or Your Users Will**

Wizards just produce a mass of code and a pretty spiffy-looking program.

**Don’t Use Wizard Code You Don’t Understand**