# UpdateActiveTime

I tend to have various development tasks running that can't be interrupted, and typically run for days at a time.
(I had a validation suite once that took weeks to run) Win10 automatic reboots to apply updates have been more than
just an annoyance, but have cost me days and weeks of work. (I feel for anybody that needs to try and run renders on a Win10 system)
There seems to be this constant cat and mouse game going on between Microsoft and power users over the ability to choose when
our systems go down for a reboot, so as fixes are found that allows us to control when the reboot happens, Microsoft comes
out with an update to disable that work around.

The one constant way to prevent unattended reboots seems to be using the Active Hours setting, but that still leaves a 12-6 hour
gap. where Win10 can go ahead and reboot.

Maimer has a good work around this (https://github.com/Maimer/update-active-hours), just run a power shell script and update your active
hours every so often so that you're always inside the set active hours. The problem I ran into with this is that my work machine (the one
where these reboots is the most costly) has a group policy on the domain that prevents running power shell scripts.

My solution was this simple C# application that simply updates the active hours each time it's run.

I've tested this out on Win10 Pro and Home 64-bit editions, and it's been working well for me. I hope you find it useful as well,
however no warrantee to the fitness of this software is provided, and if you mess up your system, get hacked because your system
is missing patches, or manage to start thermo-nuclear war, well you're on your own.

The application starts by assuming a 12-hour max active time. It tests to see if the release ID is 1803 or greater, and that it's not
the Home edition, if both of those are true then the max active time is extended to 18-hours. Since active hours can be overridden
with a group policy, it also checks for the existence of that group policy and then uses that value for max active time.

With the max active time set the current time is obtained. The active hours are then set for 1 hour prior to the current hour, and the
end time is set for <max active time> hours past that.

The app will need to be run on a regular schedule to keep the PC in the active hours window. I setup a task scheduler that runs the
app on login, and then runs every 6 hours after that.

This application should be used to give you control of WHEN you reboot your machine for Win10 updates, not to shutdown updates. Make
sure you're applying updates regularly. Use at your own risk.
