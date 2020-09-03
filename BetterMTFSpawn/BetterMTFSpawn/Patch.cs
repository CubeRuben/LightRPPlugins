using HarmonyLib;
using Respawning;
using System.Diagnostics;
using System.Reflection;

namespace BetterMTFSpawn
{
    [HarmonyPatch(typeof(RespawnManager), "ForceSpawnTeam")]
    class Patch
    {
        public static bool Prefix(RespawnManager __instance, SpawnableTeamType teamToSpawn) 
        {
            SpawnableTeamType spawnableTeamType = RespawnTickets.Singleton.DrawRandomTeam();

            if (spawnableTeamType == SpawnableTeamType.None)
            {
                return false;
            }

            if ((RespawnManager.RespawnSequencePhase)BetterMTFSpawnPlugin.CurrentSequenceRespawnManager.GetValue(__instance) == RespawnManager.RespawnSequencePhase.PlayingEntryAnimations) 
            {
                return true;
            }

            RespawnWaveGenerator.SpawnableTeams.TryGetValue(teamToSpawn, out SpawnableTeam value);
            __instance.NextKnownTeam = teamToSpawn;
            BetterMTFSpawnPlugin.CurrentSequenceRespawnManager.SetValue(__instance, RespawnManager.RespawnSequencePhase.PlayingEntryAnimations);
            BetterMTFSpawnPlugin.TimeForNextSequenceRespawnManager.SetValue(__instance, value.EffectTime);
            ((Stopwatch)BetterMTFSpawnPlugin.StopwatchRespawnManager.GetValue(__instance)).Restart();
            RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, teamToSpawn);
            return false;
        }
    }
}
