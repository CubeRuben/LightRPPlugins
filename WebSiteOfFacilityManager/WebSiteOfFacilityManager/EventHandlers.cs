using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Exiled.API.Enums;

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
            Timing.RunCoroutine(SortRoomsToGrid());
        }

        IEnumerator<float> SortRoomsToGrid() 
        {
            Log.Info("2D map creation started");

            float gridSize = ImageGenerator.ZoneGenerators[0].gridSize;

            foreach (Room room in Map.Rooms) 
            {
                switch (room.Zone)
                {
                    case ZoneType.Entrance:
                        Plugin.EZ.Minimal.x = Math.Min(Plugin.EZ.Minimal.x, room.Position.x);
                        Plugin.EZ.Minimal.y = Math.Min(Plugin.EZ.Minimal.y, room.Position.z);
                        break;
                    case ZoneType.HeavyContainment:
                        Plugin.HC.Minimal.x = Math.Min(Plugin.HC.Minimal.x, room.Position.x);
                        Plugin.HC.Minimal.y = Math.Min(Plugin.HC.Minimal.y, room.Position.z);
                        break;
                    case ZoneType.LightContainment:
                        Plugin.LC.Minimal.x = Math.Min(Plugin.LC.Minimal.x, room.Position.x);
                        Plugin.LC.Minimal.y = Math.Min(Plugin.LC.Minimal.y, room.Position.z);
                        break;
                }
            }

            Log.Info("Minimal vectors founded");

            foreach (Room room in Map.Rooms)
            {
                switch (room.Zone)
                {
                    case ZoneType.Entrance:
                        Plugin.EZ.Rooms[(int)((room.Position.x - Plugin.EZ.Minimal.x) / gridSize), (int)((room.Position.z - Plugin.EZ.Minimal.y) / gridSize)] = room;
                        break;
                    case ZoneType.HeavyContainment:
                        Plugin.HC.Rooms[(int)((room.Position.x - Plugin.HC.Minimal.x) / gridSize), (int)((room.Position.z - Plugin.HC.Minimal.y) / gridSize)] = room;
                        break;
                    case ZoneType.LightContainment:
                        Plugin.LC.Rooms[(int)((room.Position.x - Plugin.LC.Minimal.x) / gridSize), (int)((room.Position.z - Plugin.LC.Minimal.y) / gridSize)] = room;
                        break;
                }
            }

            Log.Info("2D map created");


            for (int x = 0; x < 10; x++)
            {
                string data = "";
                for (int y = 0; y < 10; y++)
                {
                    data += Convert.ToInt32(Plugin.LC.Rooms[x, y]);
                }
                Log.Info(data);
            }

            Log.Info("Debug log ended");
            yield return 1; 
        }
    }
}
