using Exiled.API.Features;
using Exiled.API.Enums;
using Exiled.API.Interfaces;
using Exiled.Events;

using System;

namespace SCP173Gate
{
    public class SCP173GatePlugin : Plugin<Config>
    {
        #region Info
        public override string Author { get; } = "CubeRuben";
        public override string Name { get; } = "SCP 173 Gate Commands";
        public override string Prefix { get; } = "SCP173G";
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(2, 0, 10);
        public override PluginPriority Priority { get; } = PluginPriority.Medium;
        #endregion

        public Door SCP173Gate;

        EventHandlers EventHandlers;

        public SCP173GatePlugin() 
        { 
        
        }

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);

            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += EventHandlers.OnSendingRemoteAdminCommand;
            Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundEnded += EventHandlers.OnRoundEnded;
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= EventHandlers.OnSendingRemoteAdminCommand;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundEnded -= EventHandlers.OnRoundEnded;

            EventHandlers = null;
        }

        public override void OnReloaded()
        {
            
        }
    }
}
