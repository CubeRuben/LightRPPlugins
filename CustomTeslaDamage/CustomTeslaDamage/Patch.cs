using HarmonyLib;
using Dissonance.Integrations.MirrorIgnorance;
using Exiled.API.Features;
using UnityEngine;

namespace CustomTeslaDamage
{
    [HarmonyPatch(typeof(PlayerStats), "CallCmdTesla")]
    class Patch
    {
        static bool Prefix(PlayerStats __instance) 
        {
            __instance.HurtPlayer(new PlayerStats.HitInfo(Random.Range(CustomTeslaDamagePlugin.Plugin.Config.TeslaDamage, CustomTeslaDamagePlugin.Plugin.Config.TeslaDamage + CustomTeslaDamagePlugin.Plugin.Config.TeslaDamageRange), __instance.GetComponent<MirrorIgnorancePlayer>().PlayerId, DamageTypes.Tesla, 0), __instance.gameObject);
            return false;
        }
    }
}
