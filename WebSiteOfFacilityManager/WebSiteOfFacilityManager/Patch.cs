using Exiled.API.Features;
using HarmonyLib;

using UnityEngine;

namespace WebSiteOfFacilityManager
{
    /*[HarmonyPatch(typeof(ImageGenerator), "PlaceRoom")]
    public class Patch
    {
        public static void Prefix(ImageGenerator __instance, Vector2 pos, ImageGenerator.ColorMap type) 
        {
            Vector2 vector = pos / 3;
            vector.x = (int)vector.x;
            vector.y = (int)vector.y;
            Log.Info(vector + "lcz");
            switch (WebSiteOfFacilityManagerPlugin.AliasOfImagaGenerator.GetValue(__instance)) 
            {
                case "LC":
                    
                    WebSiteOfFacilityManagerPlugin.LCZ[(int)pos.x, (int)pos.y] = true;
                    break;
                case "HZ":
                    WebSiteOfFacilityManagerPlugin.HCZ[(int)pos.x, (int)pos.y] = true;
                    break;
                case "EZ":
                    WebSiteOfFacilityManagerPlugin.EZ[(int)pos.x, (int)pos.y] = true;
                    break;
            }
            
        }
    }*/
}
