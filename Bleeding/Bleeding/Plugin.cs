using Exiled.API.Features;
using Exiled.API.Enums;
using Exiled.Events.Handlers;

using System;
using System.Collections.Generic;

using UnityEngine;
using System.Globalization;

namespace Bleeding
{
    public class BleedingPlugin : Plugin<Config>
    {
        #region Info
        public override string Name { get; } = "Bleeding";
        public override string Prefix { get; } = "Bleeding";
        public override string Author { get; } = "CubeRuben";
        public override PluginPriority Priority { get; } = PluginPriority.Medium;
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(2, 1, 0);
        #endregion

        public Dictionary<int, PlayerHealth> PlayersHealth;

        EventHandlers EventHandlers;
        public BleedingPlugin() 
        {
            PlayersHealth = new Dictionary<int, PlayerHealth>();
        }

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);

            Exiled.Events.Handlers.Player.Joined += EventHandlers.OnJoined;
            Exiled.Events.Handlers.Player.Left += EventHandlers.OnLeft;
            Exiled.Events.Handlers.Player.Hurting += EventHandlers.OnHurting;
            Exiled.Events.Handlers.Player.Died += EventHandlers.OnDied;
            Exiled.Events.Handlers.Player.ChangingRole += EventHandlers.OnChangingRole;
            Exiled.Events.Handlers.Player.MedicalItemUsed += EventHandlers.OnMedicalItemUsed;
            Exiled.Events.Handlers.Player.Interacted += EventHandlers.OnInteracted;
            Exiled.Events.Handlers.Player.ItemDropped += EventHandlers.OnItemDropped;
            Exiled.Events.Handlers.Player.ChangingItem += EventHandlers.OnChangingItem;
            Exiled.Events.Handlers.Player.Handcuffing += EventHandlers.OnHandcuffing;
            Exiled.Events.Handlers.Player.PickingUpItem += EventHandlers.OnPickingUpItem;

            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += EventHandlers.OnSendingRemoteAdminCommand;
            Exiled.Events.Handlers.Server.SendingConsoleCommand += EventHandlers.OnSendingConsoleCommand;
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.Joined -= EventHandlers.OnJoined;
            Exiled.Events.Handlers.Player.Left -= EventHandlers.OnLeft;
            Exiled.Events.Handlers.Player.Hurting -= EventHandlers.OnHurting;
            Exiled.Events.Handlers.Player.Died -= EventHandlers.OnDied;
            Exiled.Events.Handlers.Player.ChangingRole -= EventHandlers.OnChangingRole;
            Exiled.Events.Handlers.Player.MedicalItemUsed -= EventHandlers.OnMedicalItemUsed;
            Exiled.Events.Handlers.Player.Interacted -= EventHandlers.OnInteracted;
            Exiled.Events.Handlers.Player.ItemDropped -= EventHandlers.OnItemDropped;
            Exiled.Events.Handlers.Player.ChangingItem -= EventHandlers.OnChangingItem;
            Exiled.Events.Handlers.Player.Handcuffing -= EventHandlers.OnHandcuffing;
            Exiled.Events.Handlers.Player.PickingUpItem -= EventHandlers.OnPickingUpItem;

            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= EventHandlers.OnSendingRemoteAdminCommand;
            Exiled.Events.Handlers.Server.SendingConsoleCommand -= EventHandlers.OnSendingConsoleCommand;

            EventHandlers = null;
        }
    }
}
