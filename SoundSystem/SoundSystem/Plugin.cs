using Exiled.API.Features;
using Exiled.API.Enums;


using System;

using UnityEngine;

namespace SoundSystem
{
    public class SoundSystemPlugin : Plugin<Config>
    {
        #region Info
        public override string Name { get; } = "Sound System";
        public override string Prefix { get; } = "SndSs";
        public override string Author { get; } = "CubeRuben";
        public override PluginPriority Priority { get; } = PluginPriority.Default;
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(2, 1, 0);
        #endregion

        EventHandlers EventHandlers;

        public SoundSystemPlugin() 
        {
        
        }

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);

            Exiled.Events.Handlers.Server.SendingConsoleCommand += EventHandlers.OnSendingConsoleCommand;
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.SendingConsoleCommand -= EventHandlers.OnSendingConsoleCommand;

            EventHandlers = null;
        }
    }
}
