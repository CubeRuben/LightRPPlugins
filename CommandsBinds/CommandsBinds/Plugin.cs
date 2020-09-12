using Exiled.API.Features;
using Exiled.API.Enums;

using System;
using System.Collections.Generic;

namespace CommandsBinds
{
    public class CommandsBindsPlugin : Plugin<Config>
    {
        #region Info
        public override string Name { get; } = "Commands Binds";
        public override string Prefix { get; } = "CommmandsBinds";
        public override string Author { get; } = "CubeRuben";
        public override PluginPriority Priority { get; } = PluginPriority.Default;
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(2, 1, 0);
        #endregion

        EventHandlers EventHandlers;

        public Dictionary<int, string> PlayerToCommand;

        public CommandsBindsPlugin() 
        {
            PlayerToCommand = new Dictionary<int, string>();
        }

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);

            Exiled.Events.Handlers.Server.SendingConsoleCommand += EventHandlers.OnSendingConsoleCommand;
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.SendingConsoleCommand -= EventHandlers.OnSendingConsoleCommand;
        }
    }
}
