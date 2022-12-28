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

O()-notation image here...

**Test Your Estimates**

### Best Isn’t Always Best
You also need to be pragmatic about choosing appropriate algorithms—the fastest one is not always the best for the job. 
