using Exiled.Events.EventArgs;
using Exiled.API.Features;
using RemoteAdmin;
using System.Collections.Generic;

namespace CommandsBinds
{
    class EventHandlers
    {
        CommandsBindsPlugin Plugin;

        public EventHandlers(CommandsBindsPlugin plugin) 
        {
            Plugin = plugin;
        }

        string GetCommand(List<string> args) 
        {
            string cmd = "/";

            foreach (string a in args)
            {
                cmd += a + " ";
            }

            return cmd;
        }

        void CallCommand(string cmd, Player sender) 
        {
            GameCore.Console.singleton.TypeCommand(cmd, sender.Sender);
            sender.ShowHint("Вы вызвали команду\n" + cmd.Substring(1), 10);
        }

        public void OnSendingConsoleCommand(SendingConsoleCommandEventArgs ev) 
        {
            if (ev.Player.Group == null) 
            {
                return;
            }

            switch (ev.Name) 
            {
                case "rcall":
                    {
                        ev.Allow = false;

                        string cmd = GetCommand(ev.Arguments);
                        CallCommand(cmd, ev.Player);

                        ev.ReturnMessage = "Remote admin command called: " + cmd;
                    }
                    break;
                case "rcallcheck":
                    {
                        ev.Allow = false;
                        string cmd = GetCommand(ev.Arguments);


                        Plugin.PlayerToCommand.Remove(ev.Player.Id);
                        Plugin.PlayerToCommand.Add(ev.Player.Id, cmd);

                        ev.Player.ShowHint("Вызвать ли эту команду?\n" + cmd, 10);

                        ev.ReturnMessage = "Checking command";
                    }
                    break;
                case "rcheck":
                    {
                        ev.Allow = false;

                        if (!Plugin.PlayerToCommand.TryGetValue(ev.Player.Id, out string cmd)) 
                        {
                            break;
                        }

                        CallCommand(Plugin.PlayerToCommand[ev.Player.Id], ev.Player);

                        ev.ReturnMessage = "Remote admin command called: " + cmd;

                        Plugin.PlayerToCommand.Remove(ev.Player.Id);
                    }
                    break;
            }
        }
    }
}
