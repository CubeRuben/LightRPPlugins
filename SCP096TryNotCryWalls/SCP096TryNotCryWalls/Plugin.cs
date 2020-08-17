using Exiled.API.Features;
using Exiled.Events.Handlers;
using Exiled.API.Enums;

using System;

using HarmonyLib;
using UnityEngine;

namespace SCP096TryNotCryWalls
{
    public class SCP096TryNotCryWallsPlugin : Plugin<Config>
    {
        #region Info
        public override string Author { get; } = "CubeRuben";
        public override string Name { get; } = "SCP 096 TryNotCry Walls";
        public override string Prefix { get; } = "SCP096TNCW";
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(2, 0, 13);
        public override PluginPriority Priority { get; } = PluginPriority.Medium;
        #endregion

        Harmony Harmony;

        public SCP096TryNotCryWallsPlugin() 
        { 
            
        }

        public override void OnEnabled()
        {
            Harmony = new Harmony("com.cuberuben.scp096tncwalls");
            Harmony.PatchAll();
        }

        public override void OnDisabled()
        {
            Harmony.UnpatchAll();
        }
    }
}
