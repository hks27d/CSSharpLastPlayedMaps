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
        public override string ModuleVersion => "1.0.3";
        public override string ModuleAuthor => "HKS 27D";
        public override string ModuleDescription => "";

        readonly Queue<string> LastMapsQueue = new();
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
            {
                LastMapsQueue.Dequeue();
                LastMapsQueue.TrimExcess();
            }
        }

        [ConsoleCommand("css_lastmaps", "Last played maps")]
        [RequiresPermissions("@css/generic")]
        [CommandHelper(whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
        public void OnLastCommand(CCSPlayerController? Player, CommandInfo commandInfo)
        {
            if (Player != null && Player.IsValid && Player.PlayerPawn.IsValid && Player.Connected == PlayerConnectedState.PlayerConnected)
            {
                Player.PrintToChat($" {ChatColors.Red}Modders {ChatColors.Default}- See console for {ChatColors.Green}last maps {ChatColors.Default}output.");

                if (LastMapsQueue.Count > 0)
                {
                    int mapCounter = 0;
                    Player.PrintToConsole(" # | Date & Time         | Map");
                    Player.PrintToConsole("------------------------------");

                    foreach (string QueueElement in LastMapsQueue)
                        Player.PrintToConsole($"{++mapCounter,2} | {QueueElement}");
                }
                else
                    Player.PrintToConsole("No recent maps played.");
            }
            else
            {
                if (LastMapsQueue.Count > 0)
                {
                    int mapCounter = 0;
                    commandInfo.ReplyToCommand(" # | Date & Time         | Map");
                    commandInfo.ReplyToCommand("------------------------------");

                    foreach (string QueueElement in LastMapsQueue)
                        commandInfo.ReplyToCommand($"{++mapCounter,2} | {QueueElement}");
                }
                else
                    commandInfo.ReplyToCommand("No recent maps played.");
            }
        }
    }
}
