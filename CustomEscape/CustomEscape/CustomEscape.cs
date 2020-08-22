using Exiled.API.Features;
using Exiled.API.Enums;

using System;

namespace CustomEscape
{
    public class CustomEscapePlugin : Plugin<Config>
    {
        #region Info
        public override string Author { get; } = "CubeRuben";
        public override string Name { get; } = "Custom Escape";
        public override string Prefix { get; } = "CuEsc";
        public override Version Version { get; } = new Version(1, 0, 1);
        public override Version RequiredExiledVersion { get; } = new Version(2, 1, 0);
        public override PluginPriority Priority { get; } = PluginPriority.Last;

        #endregion

        private EventHandlers EventHandlers;

        public CustomEscapePlugin()
        {
        
        }

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);

            Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Player.Escaping += EventHandlers.OnEscaping;
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Player.Escaping -= EventHandlers.OnEscaping;

            EventHandlers = null;
        }
    }
}
