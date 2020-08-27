using Exiled.API.Features;
using Exiled.API.Enums;

using System;
using System.Collections.Generic;
using UnityEngine;
using MEC;

namespace LightSystem
{
    public class LightSystemPlugin : Plugin<Config>
    {
        #region Info
        public override string Name { get; } = "Light System";
        public override string Prefix { get; } = "LghSs";
        public override string Author { get; } = "CubeRuben";
        public override PluginPriority Priority { get; } = PluginPriority.Default;
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(2, 1, 0);
        #endregion

        EventHandlers EventHandlers;

        public List<FlickerableLightController> LCZ;
        public List<FlickerableLightController> HCZ;
        public List<FlickerableLightController> EZ;

        public LightSystemPlugin()
        {
            LCZ = new List<FlickerableLightController>();
            HCZ = new List<FlickerableLightController>();
            EZ = new List<FlickerableLightController>();
        }

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);

            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += EventHandlers.OnSendingRemoteAdminCommand;
            Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= EventHandlers.OnSendingRemoteAdminCommand;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;

            EventHandlers = null;
        }
    }
}
