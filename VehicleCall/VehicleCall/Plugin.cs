using Exiled.API.Features;
using Exiled.API.Enums;

using System;
using System.Reflection;
using Respawning;
using System.Diagnostics;
using HarmonyLib;
using LiteNetLib;
using System.Collections.Generic;

namespace VehicleCall
{
    public class VehicleCallPlugin : Plugin<Config>
    {
        #region Info
        public override string Name { get; } = "Vehicle Call";
        public override string Prefix { get; } = "VC";
        public override string Author { get; } = "CubeRuben";
        public override PluginPriority Priority { get; } = PluginPriority.Medium;
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(2, 1, 0);
        #endregion

        EventHandlers EventHandlers;
        Harmony Harmony;
        public VehicleCallPlugin() 
        { 
        
        }
        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);
            Harmony = new Harmony("com.cuberuben.patch");
            Harmony.PatchAll();
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += EventHandlers.OnSendingRemoteAdminCommand;
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= EventHandlers.OnSendingRemoteAdminCommand;
            Harmony.UnpatchAll();
            EventHandlers = null;
        }

        public void CallHelicopter() 
        {
            /*RespawnManager.Singleton.NextKnownTeam = SpawnableTeamType.NineTailedFox;
            FieldInfo a = typeof(RespawnManager).GetField("_curSequence", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            a.SetValue(RespawnManager.Singleton, RespawnManager.RespawnSequencePhase.PlayingEntryAnimations);
            FieldInfo b = typeof(RespawnManager).GetField("_stopwatch", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            ((Stopwatch)b.GetValue(RespawnManager.Singleton)).Stop();*/
            RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, SpawnableTeamType.NineTailedFox);
            FieldInfo a = typeof(RespawnManager).GetField("AllControllers", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            List<RespawnEffectsController> list = (List<RespawnEffectsController>)a.GetValue(new RespawnEffectsController());
            for () 
            { 
            
            }
            //RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.UponRespawn, SpawnableTeamType.NineTailedFox);
        }

        public void CallCar() 
        {
            RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, SpawnableTeamType.ChaosInsurgency);
        }
    }
    
    [HarmonyPatch(typeof(RespawnEffectsController), "ExecuteAllEffects")]
    public class Patch 
    {
        public static void Prefix() 
        {
            Log.Info("called");
        }
    }
}
