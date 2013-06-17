File descriptions

MCHost:
	Host.cs				C# wrapper to handle hosting the minecraft server process.

WebAApi:
	Controllers\IndexController.cs	MVC controller to handle default page and all static routes.
	Controllers\McController.cs	MVC controller to communicate with the minecraft host and
					handle collating responses from the server.
	Static\*			Static files for the web UI.

McClientWatcher:
	Program.cs			Watches for the MC server to restart (by monitoring C:\mcstarted.txt)
					and restarts a minecraft client to keep the server alive and well.
