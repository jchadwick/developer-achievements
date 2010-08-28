#Developer Achievements

##Overview
An achievements system (ala XBox Live Achievements) to encourage and reward developers for following good development practices.

##Details
Let's face it - geeks are competitive.  Nothing riles developers up more than the urge to be "better" than the coder in the cubicle next to you (except maybe a Star Trek convention... but, I digress).  Since a "better" developer is such a subjective term without statistics to back it up, the only way to resolve these debates has been through LAN gaming and passive-aggressive banter... until now. The Developer Achievements project watches your continuous integration server to capture statistics, awards developers with Achievements for accomplishing various customizable milestones (e.g. 10 broken builds!), and provides a central location for these Achievements - and thus the developers themselves - to be easily compared at a glance. 

So, wire up the system and make your bragging rights more statistically sound!


##Installation
There are two main pieces to the system:

*  __Website__
	Allows users to navigate the data in the system, most importantly the awarded Achievements. Also provides web services to be called by the various Activity listeners to log developer activity (to be turned into statistics).

*  __Listeners__
	Currently, the only Listener provided with this project is a CC.NET Task that you can easily add to your build script to run as part of your builds.  Stay tuned for more listeners to be added!