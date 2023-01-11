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