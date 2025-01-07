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
        public override string ModuleVersion => "1.0.0";
        public override string ModuleAuthor => "HKS 27D";
        public override string ModuleDescription => "";

#pragma warning disable IDE0044 // Add readonly modifier
        Queue<string> LastMapsQueue = new();
        Queue<string> LastMapsQueueReversed = new();
#pragma warning restore IDE0044 // Add readonly modifier
        const int MaxLastMapsElements = 20;

        public override void Load(bool hotReload)
        {
            RegisterListener<Listeners.OnMapStart>(mapName =>
            {
                DateTime currentDateTime = DateTime.Now;
                string formattedTime = currentDateTime.ToString("yyyy-MM-dd, HH:mm:ss");
                AddMapIntoQueue(mapName, formattedTime);
                LastMapsQueueReversed = new(LastMapsQueue);
                ReverseQueue(LastMapsQueueReversed);
            });
        }

        private void AddMapIntoQueue(string map, string fTime)
        {
            LastMapsQueue.Enqueue($"{map} ({fTime})");
            if (LastMapsQueue.Count > MaxLastMapsElements)
                LastMapsQueue.Dequeue();
        }

        private static void ReverseQueue(Queue<string> queue)
        {
            List<string> tempList = new(queue);
            queue.Clear();

            for (int i = tempList.Count - 1; i >= 0; i--)
            {
                queue.Enqueue(tempList[i]);
            }
        }

        [ConsoleCommand("css_lastmaps", "Last played maps")]
        [RequiresPermissions("@css/generic")]
        [CommandHelper(whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
        public void OnLastCommand(CCSPlayerController? Player, CommandInfo commandInfo)
        {
            if (Player != null && Player.IsValid && Player.PlayerPawn.IsValid && Player.Connected == PlayerConnectedState.PlayerConnected)
            {
                Player.PrintToChat($" {ChatColors.Red}Modders {ChatColors.Default}- See console for {ChatColors.Green}css_lastmaps {ChatColors.Default}output.");
                Player.PrintToConsole("+------------------+");
                Player.PrintToConsole("| LAST PLAYED MAPS |");
                Player.PrintToConsole("+------------------+");

                if (LastMapsQueue.Count > 0)
                {
                    int mapCounter = 1;

                    foreach (string QueueElement in LastMapsQueueReversed)
                    {
                        Player.PrintToConsole($"{mapCounter}. {QueueElement}");
                        mapCounter++;
                    }
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

                    foreach (string QueueElement in LastMapsQueueReversed)
                    {
                        commandInfo.ReplyToCommand($"{mapCounter}. {QueueElement}");
                        mapCounter++;
                    }
                }
                else
                    commandInfo.ReplyToCommand("No recent maps played.");
            }
        }
    }
}
