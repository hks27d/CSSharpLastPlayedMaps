#  LastMap
Get the last played maps.

<img width="492" height="161" alt="image" src="https://github.com/user-attachments/assets/54b2c421-9132-4eff-a2c5-d5ff09e39d89" />


##  Description
Shows a list of the last played maps. It has console support. It also uses built-in Queues from .NET.

##  Dependecies
- [CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp)

##  Installation
1. Build the project with `dotnet build -c Release`.
2. Copy the generated `LastMap.dll` into `/game/csgo/addons/counterstrikesharp/plugins/LastMap`.
3. Restart the server or reload the plugin.

At runtime, CounterStrikeSharp creates a JSON config for the plugin with a `MaxSavedMaps` setting. Its default is `5`; change that value to set the maximum number of maps retained in memory. The plugin records the currently active map when it loads, then records each map change. Config changes trim the current history when parsed. The `css_lastmap` command displays recorded maps newest first. Players need the `@css/generic` permission; the server console can also run the command.

## Code changes
If you want to change the plugin settings:
- `[ConsoleCommand("css_lastmap", "Display the most recently played maps")]` - console command
- `[RequiresPermissions("@css/generic")]` - admin access
- `MaxSavedMaps` in the generated config - maximum number of recorded maps (default: `5`)

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

##  Show your support
Give a if this project helped you.

## 📝 License
MIT based on outlined exemption from CounterStrikeSharp.
