# Chapter 4 - Pragmatic Paranoia
**You can't write perfect software**
Noone writes perfect code, including themselves. Defend against your own mistakes.

## Design by Contract
One of the solutions for ensuring plain dealing is the __contract__. A contract defines your rights and responsibilities.

Documenting and verifying program's claims is the heart of __Design by Contract (DBC)__.

Expectations and claims:
- Preconditions. What must be true when the routine to be called.
- Postconditions. The state of the world when the routine is done.
- Class invariants. A class ensures that this condition is always true from the perspective of a caller.

Be strict in what you will accept before you begin, and promise as little as possible in return.

**Liskov Substitution Principle**: Subclasses must be usable through the base class interface without the need for the user to know the difference. A subclass may, optionally, accept a wider range of input, or make stronger guarantees. But it **must** accept at least as much, and guarantee as much, as its parent.

Even without automatic checking, you can put the contract in the code **as comments** and still get a very real benefit. If nothing else, the commented contracts give you **a place to start looking** when trouble strikes.

You can partially emulate checking contract in some languages by using __assertions__.

Built-in DBC support: Eiffel, Sather. C, C++ and Java can use preprocessors. For C and C++: __Nana__. For Java: __iContract__.

Be sure not to confuse requirements that are fixed, **inviolate laws** with those that are **merely policies** that might change with a new management regime.

__The banana problem, fencepost error, off-by-one error__ - errors of boundary conditions.

## Dead Programs Tell No Lies
It’s easy to fall into the **“it can’t happen”** mentality. You could convince yourself that the error can’t happen, and choose to ignore it. Instead, Pragmatic Programmers tell themselves that if there is an error, something very, **very bad** has happened.

**Crash Early**

One of the benefits of detecting problems as soon as you can is that you can crash earlier. A **dead program** normally does a lot less damage than a **crippled** one.

## Assertive Programming
**If It Can’t Happen, Use Assertions to Ensure That It Won’t**
Your first line of defense is checking for any possible error, and your second is using assertions to try to detect those you’ve missed.
**Heisenbug** is a software bug that seems to disappear or alter its behavior when one attempts to study it.

## When to use exceptions
**Use Exceptions For Exceptional Problems**

## How to Balance Resources
**Finish what you start** - ideally, the routine that allocates a resource should also free it.

- Deallocate resources in the opposite order to that in which you allocate them.
- When allocating the same set of resources in different places in your code, always allocate them in the same order.