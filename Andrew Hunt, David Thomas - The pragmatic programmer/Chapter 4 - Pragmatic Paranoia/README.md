# Chapter 4 - Pragmatic Paranoia
**You can't write perfect software**
Noone writes perfect code, including themselves. Defend againsta your own mistakes.

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