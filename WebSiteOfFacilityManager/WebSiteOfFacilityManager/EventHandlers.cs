using Exiled.API.Features;
using Exiled.Events.EventArgs;

using UnityEngine;

using MEC;
using System.Collections.Generic;
using System;

namespace WebSiteOfFacilityManager
{
    class EventHandlers
    {
        WebSiteOfFacilityManagerPlugin Plugin;

        public EventHandlers(WebSiteOfFacilityManagerPlugin plugin)
        {
            Plugin = plugin;
        }

        public void OnWaitingForPlayers()
        {
            for (int x = 0; x < 10; x++) 
            {
                string data = "";
                for (int y = 0; y < 10; y++)
                {
                    data += Convert.ToInt32(WebSiteOfFacilityManagerPlugin.LCZ[x, y]);
                }
                Log.Info(data);
            }
            Timing.RunCoroutine(SortRoomsToGrid());
        }

        IEnumerator<float> SortRoomsToGrid() 
        {
            List<ImageGenerator.MinimapElement> minimap = (List<ImageGenerator.MinimapElement>)Plugin.MinimapField.GetValue(ImageGenerator.ZoneGenerators[0]);

            foreach (ImageGenerator.MinimapElement room in minimap) 
            {
                Log.Info(room.position);
            }


            foreach (ImageGenerator.ColorMap colorMap in ImageGenerator.ZoneGenerators[0].colorMap) 
            {
                Log.Info(colorMap.centerOffset);
            }
            yield return 1; 
        }
    }
}
