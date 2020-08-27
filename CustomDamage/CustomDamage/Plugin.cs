using Exiled.API.Features;
using Exiled.API.Enums;

using System;
using System.Collections.Generic;

namespace CustomDamage
{
    public class CustomDamagePlugin : Plugin<Config>
    {
        #region Info
        public override string Name { get; } = "Custom Damage";
        public override string Prefix { get; } = "CuDMG";
        public override string Author { get; } = "Cube Ruben";
        public override PluginPriority Priority { get; } = PluginPriority.Default;
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(2, 1, 0);
        #endregion

        EventHandlers EventHandlers;

        public Dictionary<int, float> PlayerToCustomDamage;
        public CustomDamagePlugin() 
        {
            PlayerToCustomDamage = new Dictionary<int, float>();
        }

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);

            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += EventHandlers.OnSendingRemoteAdminCommand;
            Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;

            Exiled.Events.Handlers.Player.Hurting += EventHandlers.OnHurting;
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= EventHandlers.OnSendingRemoteAdminCommand;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;

            Exiled.Events.Handlers.Player.Hurting -= EventHandlers.OnHurting;
        }
    }
}
