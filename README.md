McDecision
==========

Decision engine written in Minecraft


This project was written for the OMGWTF2 contest.  It is a decision engine written in the most obtuse way possible.

It works like this:

The ASP.Net MVC 4 application hosts a minecraft (java) process via a static variable in global.asax.cs.  This 
variable is shared with the rest of the application using HttpContext.Current.Application[""].

ASP.Net MVC 4 application (with some WTFs of it's own) receives a request for /decision

The MVC4 application asks the host application to begin the decision-making process.

The Host process sends commands via STDIN to minecraft.

Minecraft detects the incoming command and kicks off a not-so-elaborate (have you SEEN some of the redstone stuff 
people make?) redstone circuit to determine the outcome of the decision.  Command blocks at the end of the decision
circuit sends a "/say ..." command to voice steve's opinion on the decision.

The Host process detects the command via STDERR (wtf?) and raises an event back to the MVC process.

The MVC process sends the resulting decision back to the caller.



This entire process is wrapped up in a little AJAX web page that asks for a decision on a button click and displays a
picture of steve saying "yes" or "no", with a background of my first screenshot trying to get all the interoperability
working...


I am hosting both the web application and the minecraft server locally, accessible to the internet; though I prefer to 
limit who I give the IP address to :)  Judges are welcome, and if you don't have a minecraft account (so you can see
the action) I have a spare one you can borrow.



