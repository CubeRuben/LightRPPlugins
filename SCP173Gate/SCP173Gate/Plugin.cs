using Exiled.API.Features;
using Exiled.API.Enums;
using Exiled.API.Interfaces;
using Exiled.Events;

using UnityEngine;

using System;
using LiteNetLib;

namespace SCP173Gate
{
    public class SCP173GatePlugin : Plugin<Config>
    {
        #region Info
        public override string Author { get; } = "CubeRuben";
        public override string Name { get; } = "SCP 173 Gate Commands";
        public override string Prefix { get; } = "SCP173G";
        public override Version Version { get; } = new Version(1, 0, 1);
        public override Version RequiredExiledVersion { get; } = new Version(2, 0, 13);
        public override PluginPriority Priority { get; } = PluginPriority.Medium;
        #endregion

        public Door SCP173Gate;

        public Vector3 SCP173ObservationPostEnterPosition;

        public Vector3 SCP173HallWayCameraPosition;

        public GameObject[] Barriers = new GameObject[2];

        EventHandlers EventHandlers;

        public SCP173GatePlugin() 
        { 
        
        }

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);

            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += EventHandlers.OnSendingRemoteAdminCommand;
            Exiled.Events.Handlers.Server.SendingConsoleCommand += EventHandlers.OnConsoleCommand;
            Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundEnded += EventHandlers.OnRoundEnded;
            Exiled.Events.Handlers.Player.Hurting += EventHandlers.OnHurting;
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= EventHandlers.OnSendingRemoteAdminCommand;
            Exiled.Events.Handlers.Server.SendingConsoleCommand -= EventHandlers.OnConsoleCommand;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundEnded -= EventHandlers.OnRoundEnded;
            Exiled.Events.Handlers.Player.Hurting -= EventHandlers.OnHurting;

            EventHandlers = null;
        }
    }
}
