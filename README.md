[![Discord](https://img.shields.io/discord/1323042994437357599?style=for-the-badge)](https://discord.com/invite/zsmUzthPXx)

# 📌 CS# Last Played Maps (console support)
Get the last played maps.

## 🌐 Description
Shows a list of the last played maps. It has console support. It also uses built-in Queues from .NET and a basic func to do queue reversal.

## 📗 Dependecies
- [CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp)

## 📁 Installation
1. Get an IDE & Code Editor (Visual Studio, Rider or others).
2. Install package 'Microsoft.Extensions.Logging.Abstractions'.
3. Edit and debug if you need to.
4. Build the code.
5. Add the files (`.dll`, `.pdb`, `.deps.json`) in their own directory.
6. Rename directory. Use the same name.
7. Upload directory in `/game/csgo/addons/counterstrikesharp/plugins`

## 📄 Code changes
If you just want to modify general settings of the plugin:
- `[ConsoleCommand("css_lastmaps", "Last played maps")]` - console command
- `[RequiresPermissions("@css/generic")]` - admin access
- `const int LastMapsMaxElements = 20;` - max queue elements

## 🤝 Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## ⭐ Show your support
Give a ⭐ if this project helped you.

## 📝 License
MIT based on outlined exemption from CounterStrikeSharp.
