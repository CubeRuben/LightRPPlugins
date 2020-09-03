using Exiled.API.Features;
using Exiled.API.Enums;
using System;
using HarmonyLib;
using System.Runtime.Remoting.Messaging;
using System.Runtime.CompilerServices;

namespace CustomTeslaDamage
{
    public class CustomTeslaDamagePlugin : Plugin<Config>
    {
        public override string Name { get; } = "Custom Tesla Damage";
        public override string Prefix { get; } = "CTD";
        public override string Author { get; } = "CubeRuben";
        public override PluginPriority Priority { get; } = PluginPriority.Low;
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(2, 1, 0);

        Harmony Harmony;
        public static CustomTeslaDamagePlugin Plugin;

        public CustomTeslaDamagePlugin() 
        {
            Plugin = this;
        }
        public override void OnEnabled()
        {
            Harmony = new Harmony("com.cuberuben.customtesladamage");
            Harmony.PatchAll();
        }

        public override void OnDisabled()
        {
            Harmony.UnpatchAll();
        }
    }
}
