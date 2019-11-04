# Blazor-bazed web UI

The UI is client-only, there is no server-side logic.
Only static files are produced to be served by any web server.

## Build and run

* (optional) `dotnet tool install -g dotnet-serve` installs a simple static file server
* `dotnet publish -c Release`
* `cd bin/Release/netstandard2.0/publish/Klopodavka.BlazorUi/dist`
* `dotnet serve`