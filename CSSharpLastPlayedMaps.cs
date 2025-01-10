using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace CSSharpLastPlayedMaps
{
    public class CSSharpLastPlayedMaps : BasePlugin
    {
        public override string ModuleName => "CS# Last Played Maps (console support)";
        public override string ModuleVersion => "1.0.1";
        public override string ModuleAuthor => "HKS 27D";
        public override string ModuleDescription => "";

#pragma warning disable IDE0044 // Add readonly modifier
        Queue<string> LastMapsQueue = new();
#pragma warning restore IDE0044 // Add readonly modifier
        const int MaxLastMapsElements = 20;

        public override void Load(bool hotReload)
        {
            RegisterListener<Listeners.OnMapStart>(mapName =>
            {
                AddMapIntoQueue(mapName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            });
        }

        private void AddMapIntoQueue(string map, string fTime)
        {
            LastMapsQueue.Enqueue($"{fTime,19} | {map}");
            if (LastMapsQueue.Count > MaxLastMapsElements)
                LastMapsQueue.Dequeue();
        }

        [ConsoleCommand("css_lastmaps", "Last played maps")]
        [RequiresPermissions("@css/generic")]
        [CommandHelper(whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
        public void OnLastCommand(CCSPlayerController? Player, CommandInfo commandInfo)
        {
            if (Player != null && Player.IsValid && Player.PlayerPawn.IsValid && Player.Connected == PlayerConnectedState.PlayerConnected)
            {
                Player.PrintToChat($" {ChatColors.Red}Modders {ChatColors.Default}- See console for {ChatColors.Green}last maps {ChatColors.Default}output.");
                Player.PrintToConsole("+------------------+");
                Player.PrintToConsole("| LAST PLAYED MAPS |");
                Player.PrintToConsole("+------------------+");

                if (LastMapsQueue.Count > 0)
                {
                    int mapCounter = 1;
                    foreach (string QueueElement in LastMapsQueue)
                        Player.PrintToConsole($"{++mapCounter,2} | {QueueElement}");
                }
                else
                    Player.PrintToConsole("No recent maps played.");
            }
            else
            {
                commandInfo.ReplyToCommand("+------------------+");
                commandInfo.ReplyToCommand("| LAST PLAYED MAPS |");
                commandInfo.ReplyToCommand("+------------------+");

                if (LastMapsQueue.Count > 0)
                {
                    int mapCounter = 1;
                    foreach (string QueueElement in LastMapsQueue)
                        commandInfo.ReplyToCommand($"{++mapCounter,2} | {QueueElement}");
                }
                else
                    commandInfo.ReplyToCommand("No recent maps played.");
            }
        }
    }
}
