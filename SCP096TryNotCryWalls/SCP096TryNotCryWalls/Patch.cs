using HarmonyLib;
using UnityEngine;
using Mirror;
using System;

namespace SCP096TryNotCryWalls
{
    [HarmonyPatch(typeof(PlayableScps.Scp096), "TryNotToCry")]
    public class Patch
    {
        static void Prefix(PlayableScps.Scp096 __instance) 
        {
            if (!NetworkServer.active)
            {
                throw new InvalidOperationException("Called TryNotToCry from client.");
            }

            if (Physics.Raycast(__instance.Hub.PlayerCameraReference.position, __instance.Hub.PlayerCameraReference.forward, out RaycastHit hitInfo, 1f, LayerMask.GetMask("Glass", "BreakableGlass", "Default")))
            {
                __instance.PlayerState = PlayableScps.Scp096PlayerState.TryNotToCry;
            }
        }
    }
}
