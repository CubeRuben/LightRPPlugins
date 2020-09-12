using Exiled.API.Features;
using Exiled.API.Enums;

using System;

namespace AdminAlarm
{
    public class AdminAlarmPlugin : Plugin<Config>
    {
        #region Info
        public override string Name { get; } = "Admin Alarm Plugin";
        public override string Prefix { get; } = "AdminAlarm";
        public override string Author { get; } = "CubeRuben";
        public override PluginPriority Priority { get; } = PluginPriority.Default;
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(2, 1, 0);
        #endregion

        EventHandlers EventHandlers;

        public AdminAlarmPlugin() 
        { 
        
        }

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);

            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += EventHandlers.OnSendingRemoteAdminCommand;
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= EventHandlers.OnSendingRemoteAdminCommand;
        }
    }
}
