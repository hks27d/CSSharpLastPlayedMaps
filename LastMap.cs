using System.Text.Json.Serialization;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;

namespace LastMap
{
    public class LastMapConfig : BasePluginConfig
    {
        public const int DefaultMaxSavedMaps = 5;

        [JsonPropertyName("MaxSavedMaps")]
        public int MaxSavedMaps { get; set; } = DefaultMaxSavedMaps;
    }

    public class LastMap : BasePlugin, IPluginConfig<LastMapConfig>
    {
        public override string ModuleName => "LastMap";
        public override string ModuleVersion => "1.0.0";
        public override string ModuleAuthor => "GSM-RO";
        public override string ModuleDescription => "Displays the most recently played maps.";

        public LastMapConfig Config { get; set; } = null!;
        private readonly Queue<MapHistoryEntry> _lastMaps = new();

        public void OnConfigParsed(LastMapConfig config)
        {
            if (config.MaxSavedMaps < 1)
            {
                Logger.LogWarning(
                    "MaxSavedMaps must be at least 1. Using the default value of {DefaultMaxSavedMaps}.",
                    LastMapConfig.DefaultMaxSavedMaps);
                config.MaxSavedMaps = LastMapConfig.DefaultMaxSavedMaps;
            }

            Config = config;
            TrimHistory();
        }

        public override void Load(bool hotReload)
        {
            RegisterListener<Listeners.OnMapStart>(mapName => AddMap(mapName));

            var currentMap = Server.MapName;
            if (!string.IsNullOrWhiteSpace(currentMap))
            {
                AddMap(currentMap);
            }
        }

        private void AddMap(string map)
        {
            _lastMaps.Enqueue(new MapHistoryEntry(map, DateTime.Now));
            TrimHistory();
        }

        private void TrimHistory()
        {
            while (_lastMaps.Count > Config.MaxSavedMaps)
            {
                _lastMaps.Dequeue();
            }
        }

        [ConsoleCommand("css_lastmap", "Display the most recently played maps")]
        [RequiresPermissions("@css/generic")]
        [CommandHelper(whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
        public void OnLastMapCommand(CCSPlayerController? player, CommandInfo commandInfo)
        {
            var recentMaps = _lastMaps.Reverse().ToArray();

            if (player is { IsValid: true })
            {
                player.PrintToChat($"{ChatColors.Green}LastMap{ChatColors.Default}: see your console for recent maps.");
                PrintHistory(recentMaps, player.PrintToConsole);
            }
            else
            {
                PrintHistory(recentMaps, commandInfo.ReplyToCommand);
            }
        }

        private static void PrintHistory(
            IReadOnlyList<MapHistoryEntry> maps,
            Action<string> print)
        {
            if (maps.Count == 0)
            {
                print("[LastMap] No recent maps have been recorded yet.");
                return;
            }

            print("[LastMap]  # | Date & Time         | Map");
            print("[LastMap] ------------------------------");

            for (var i = 0; i < maps.Count; i++)
            {
                var map = maps[i];
                print($"[LastMap] {i + 1,2} | {map.StartedAt:yyyy-MM-dd HH:mm:ss} | {map.Name}");
            }
        }

        private readonly record struct MapHistoryEntry(string Name, DateTime StartedAt);
    }
}
