using Exiled.API.Features;
using HarmonyLib;

using UnityEngine;

namespace WebSiteOfFacilityManager
{/*
    [HarmonyPatch(typeof(ImageGenerator), "PlaceRoom")]
    public class Patch
    {
        /*public static void Prefix(ImageGenerator __instance, Vector2 pos, ImageGenerator.ColorMap type) 
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

        public static bool Prefix(ImageGenerator __instance, Vector2 pos, ImageGenerator.ColorMap type) 
        {
			string text = "";
			try
			{
				text = "ERR#1 (marking bitmap)";
				__instance.BlankSquare(pos);
				Room room = null;
				text = "ERR#2 (looping)";
				do
				{
					text = "ERR#3 (randomizing)";
					int index = UnityEngine.Random.Range(0, roomsOfType[(int)type.type].roomsOfType.Count);
					text = $"ERR#4 ({roomsOfType[(int)type.type].roomsOfType.Count} rooms remaining)";
					room = roomsOfType[(int)type.type].roomsOfType[index];
					if (room.room.Count == 0)
					{
						text = "ERR#5 (randomizing)";
						roomsOfType[(int)type.type].roomsOfType.RemoveAt(index);
					}
				}
				while (room.room.Count == 0);
				room.room[0].transform.localPosition = new Vector3(pos.x * gridSize / 3f, height, pos.y * gridSize / 3f) + offset;
				room.room[0].transform.localRotation = Quaternion.Euler(Vector3.up * (type.rotationY + y_offset));
				text = "ERR#6 (preparing minimap)";
				if (minimapTarget != null)
				{
					MinimapLegend minimapLegend = null;
					MinimapLegend[] array = legend;
					foreach (MinimapLegend minimapLegend2 in array)
					{
						if (room.room[0].name.Contains(minimapLegend2.containsInName))
						{
							minimapLegend = minimapLegend2;
						}
					}
					if (minimapLegend != null)
					{
						minimap.Add(new MinimapElement
						{
							icon = minimapLegend.icon,
							position = pos,
							roomName = minimapLegend.label,
							rotation = (int)type.rotationY,
							roomSource = room.room[0].gameObject
						});
					}
				}
				text = "ERR#7 (list element removal)";
				room.room[0].SetActive(value: true);
				room.room.RemoveAt(0);
			}
			catch (Exception ex)
			{
				RandomSeedSync.DebugError("Failed to generate a room of " + alias + " zone (TYPE#" + type.type.ToString() + "). Error code - " + text + " | Debug info - " + ex.Message);
			}
		}
    }*/
}
