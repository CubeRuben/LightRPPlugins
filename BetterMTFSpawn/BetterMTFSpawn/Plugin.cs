using Exiled.API.Features;
using Exiled.API.Enums;

using System;
using System.Reflection;

using HarmonyLib;
using Respawning;

namespace BetterMTFSpawn
{
    public class BetterMTFSpawnPlugin : Plugin<Config>
    {
        #region Info
        public override string Name { get; } = "Better MTF Spawn";
        public override string Prefix { get; } = "BMTFS";
        public override string Author { get; } = "CubeRuben";
        public override PluginPriority Priority { get; } = PluginPriority.Lower;
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(2, 1, 0);
        #endregion

        Harmony Harmony;

        public static FieldInfo CurrentSequenceRespawnManager;
        public static FieldInfo StopwatchRespawnManager;
        public static FieldInfo TimeForNextSequenceRespawnManager;
        public BetterMTFSpawnPlugin() 
        {
            Type typeOfRespawnManager = typeof(RespawnManager);
            BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
            CurrentSequenceRespawnManager = typeOfRespawnManager.GetField("_curSequence", bindingFlags);
            StopwatchRespawnManager = typeOfRespawnManager.GetField("_stopwatch", bindingFlags);
            TimeForNextSequenceRespawnManager = typeOfRespawnManager.GetField("_timeForNextSequence", bindingFlags);
        }

        public override void OnEnabled()
        {
            Harmony = new Harmony("com.cuberuben.bettermtfspawn");
            Harmony.PatchAll();
        }

        public override void OnDisabled()
        {
            Harmony.UnpatchAll();
        }
    }
}
