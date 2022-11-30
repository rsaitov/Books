# Chapter 2 - A Pragmatic Approach

Every piece of knowledge must have a single, unambiguous, authoritative representation within a system.

__DRY—Don’t Repeat Yourself__

The alternative is to have the same thing expressed in two or more places. If you change one, you have to remember to change the others. It isn't a question of whether you'll remember: it's a question of when you'll forget.

Most of the duplication we see falls into one of the following categories:
- **Imposed duplication**. Developers feel they have no choice - the environment seems to require duplication.
- **Inadvertent duplication**. Developers don't realize that they are duplicationg information.
- **Impatient duplication**. Developers get lazy and duplicate because it seems easier. Spend time up front to save pain later.
- **Interdeveloper duplication**. Multiple people on a team (or on different teams) duplicate a piece of information. Make code easy to reuse. If it isn't easy, people won't do it. And if you fail to reuse, you risk duplicationg knowledge.

## Orthogonality

**Orthogonality** is a critical concept if you want to produce ssytems that are easy to design, build, test and extend. In computing two or more things are orthogonal if changes in one do not affect any of the others.

Two major benefits of writing orthogonal systems: increased productivity and reduced risk.

Techniques you can use to maintain orthogonality:
- **Keep your code decoupled**. MOdules that don't reveal anything unnecessary to other modules and that don't rely on other modules' implementations.
- **Avoid global data**. Every time your code references global data, it ties itself into the other components that share that data.
- **Avoid similar functions**. Maybe functions share common code at the start and end, but each has a different central algorithm.

Get into the habit of being constantly critical of your code. Look for any opportunities to reorganize it , to improve its structure and orthogonality. This process is called __refactoring__.

__Testing__ is a good point to assume system's orthogonality. Do you change just one module, or are the changes scattered througnout the entire system?

With __DRY__ you're looking to minimize duplication within a system, whereas with __orthogonality__ you reduce the interdependency among the system's components.

## Reversibility

Once you decided to use the vendor's database or that architectural pattern, or a certain deployment model, you are commited to a course of action that cannot be undone, except at great expense.

There are no final decisions.

While many people try to keep their code flexible, you also need to think about maintaining flexibility in the areas of architecture, deployment, and vendor integration.

No one knows what the future may hold, especially not us! So enable your code to rock-n-roll: to "rock on" when it can, to roll with the punches when it must.

## Tracer bullets

Tracer bullets are preferred to the labor of calculation. The feedback is immediate, and because they operate in the same environment as the real ammunition, external effects are minimized.

Tracer code is not disposable: you write it for keeps. It contains all the error checking, strcuturing, documentatiom, and self-checking that any piece of production code has. It simply is not fully functional.

The tracer code approach has many advantages:

- **Users get to see something working early.**
- **Developers build a structure to work in.**
- **You have an integration platform.**
- **You have something to demonstrate.**
- **You have a better feel for progress.**

Tracer bullets don't always hit their target. It's the same with tracer code.
