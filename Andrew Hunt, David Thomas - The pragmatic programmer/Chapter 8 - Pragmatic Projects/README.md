# Chapter 8 - Pragmatic Projects
## Pragmatic Teams
Pragmatic techniques help an individual be a better programmer. But these methods work also for teams as well. Recast some of the previous sections:
- **No broken windows**. The team __must__ take responsibility for the quality of the product. Quality can come only from the individual contributions of __all__ team members.
- **Boiled Frogs**. Make sure everyone actively monitors the environment for changes. Maybe appoint a __chief water tester__. Have this person check constantly for increased scope, decreased time scales, additional features, new environments—anything that wasn’t in the original agreement.
- **Communicate**. The team as an entity needs to communicate clearly with the rest of the world. Generate a **brand**.
- **Don't repeat yourself**. Some teams appoint a member as the **project librarian**, responsible for coordinating documentation and code repositories. When the project’s too big for one librarian (or when no one wants to play the role), appoint people as focal points for various functional aspects of the work.
- **Orthogonality**. Organize around Functionality, not job functions.
- **Automation**. Automate everything the team does. To ensure that things get automated, appoint one or more team members as tool builders to construct and deploy the tools that automate the project **drudgery**.
- **Know when to stop addin paint**. Give each member the ability to shine in his or her own way.

## Ubiquitous Automation

**Civilization advances by extending the number of important operations we can perform without thinking.** __Alfred North Whitehead__

Manual procedures leave consistency up to chance; repeatability isn’t guaranteed, especially if aspects of the procedure are open to interpretation by different people.

**Don’t Use Manual Procedures**

A shell script or batch file will execute the same instructions, in the same order, time after time. It can be put under source control, so you can examine changes to the procedure over time as well.

Use schedulers (cron, at) to run scripts periodically - unattended, automatically.

Make __night builds__ to run all available tests and intercept bugs as early as possible.

__Final builds__, which you intend to ship as products, may have different requirements from the regular nightly build.

Web content should be generated automatically from information in the repository and published __without__ human intervention.

### The Cobbler’s Children
Often, people who develop software use the poorest tools to do the job. Let the computer do the repetitious, the mundane — it will do a better job of it than we would. We’ve got more important and more difficult things to do.

## Ruthless Testing
Finding bugs is somewhat like fishing with a net. We use fine, small nets (unit tests) to catch the minnows, and big, coarse nets (integration tests) to catch the killer sharks.
**Test Early. Test Often. Test Automatically**

The earlier a bug is found, the cheaper it is to remedy.

**"Code a little, test a little"** __Smalltalk__.

### What To Test
There are seceral major types of software testing that you need to perform:
- **Unit testing**. A unit test is code that exercises a module.
- **Integration testing**. Testing how entire subsystems honor their contracts.
- **Validation and verification**. The users told you what they wanted, but is it what they need?
- **Resource exhaustion, errors, and recovery**. Now that you have a pretty good idea that the system will behave correctly under __ideal conditions__, you need to discover how it will behave under __real-world__ conditions.
- **Performance testing**. Ask yourself if the software meets the performance requirements under real-world conditions—with the expected number of users, or connections, or transactions per second. Is it scalable? 
- **Usability testing**. It is performed with real users, under real environmental conditions.

## How To Test
- **Regression testing**. A regression test compares the output of the current test with previous (or known) values. We can ensure that bugs we fixed today didn’t break things that were working yesterday. 
- **Test data**. There are only two kinds of data: real-world data and synthetic data. We actually need to use both, because the different natures of these kinds of data will
expose different bugs in our software. You need a lot of data, possibly __more than any real-world sample__ could provide. You need data to stress the __boundary conditions__.
- **Exercising GUI systems**. Testing GUI-intensive systems often requires specialized testing tools. These tools may be based on a simple event capture/playback model,
or they may require specially written scripts to drive the GUI. Some systems combine elements of both.
- **Testing the tests**. After you have written a test to detect a particular bug, cause the bug deliberately and make sure the test complains. Use Saboteurs to Test Your Testing.
- **Testing thoroughly**. Use coverage analysis tools to keep track of which lines of code have been executed and which haven't. Even if you do happen to hit every line of code, that’s not the whole picture. What is important is __the number of states__ that your program may haveTest State Coverage, Not Code Coverage.

### When To Test
As soon as any production code exists, it needs to be tested.

But some tests may not be easily run on a such a frequent basis. Stress tests, for instance, may require special setup or equipment, and some hand holding. These tests may be run less often—weekly or monthly, perhaps. But it is important that they be run on a regular, scheduled basis. If it can’t be done automatically, then make sure it appears on
the schedule, with all the necessary resources allocated to the task.

## Tightening the Net
If a bug slips through the net of existing tests, you need to add a new test to trap it next time.