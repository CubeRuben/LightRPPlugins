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
                        WebSiteOfFacilityManagerPlugin.EZ.Minimal.x = Math.Min(WebSiteOfFacilityManagerPlugin.EZ.Minimal.x, room.Position.x);
                        WebSiteOfFacilityManagerPlugin.EZ.Minimal.y = Math.Min(WebSiteOfFacilityManagerPlugin.EZ.Minimal.y, room.Position.z);
                        break;
                    case ZoneType.HeavyContainment:
                        WebSiteOfFacilityManagerPlugin.HC.Minimal.x = Math.Min(WebSiteOfFacilityManagerPlugin.HC.Minimal.x, room.Position.x);
                        WebSiteOfFacilityManagerPlugin.HC.Minimal.y = Math.Min(WebSiteOfFacilityManagerPlugin.HC.Minimal.y, room.Position.z);
                        break;
                    case ZoneType.LightContainment:
                        WebSiteOfFacilityManagerPlugin.LC.Minimal.x = Math.Min(WebSiteOfFacilityManagerPlugin.LC.Minimal.x, room.Position.x);
                        WebSiteOfFacilityManagerPlugin.LC.Minimal.y = Math.Min(WebSiteOfFacilityManagerPlugin.LC.Minimal.y, room.Position.z);
                        break;
                }
            }

            WebSiteOfFacilityManagerPlugin.EZ.Minimal.x -= gridSize;
            WebSiteOfFacilityManagerPlugin.EZ.Minimal.y -= gridSize;
            WebSiteOfFacilityManagerPlugin.HC.Minimal.x -= gridSize;
            WebSiteOfFacilityManagerPlugin.HC.Minimal.y -= gridSize;
            WebSiteOfFacilityManagerPlugin.LC.Minimal.x -= gridSize;
            WebSiteOfFacilityManagerPlugin.LC.Minimal.y -= gridSize;

            Log.Info("Minimal vectors founded");

            foreach (Room room in Map.Rooms)
            {
                switch (room.Zone)
                {
                    case ZoneType.Entrance:
                        WebSiteOfFacilityManagerPlugin.EZ.Rooms[(int)Math.Round((room.Position.x - WebSiteOfFacilityManagerPlugin.EZ.Minimal.x) / gridSize), (int)Math.Round((room.Position.z - WebSiteOfFacilityManagerPlugin.EZ.Minimal.y) / gridSize)] = room;
                        break;
                    case ZoneType.HeavyContainment:
                        WebSiteOfFacilityManagerPlugin.HC.Rooms[(int)Math.Round((room.Position.x - WebSiteOfFacilityManagerPlugin.HC.Minimal.x) / gridSize), (int)Math.Round((room.Position.z - WebSiteOfFacilityManagerPlugin.HC.Minimal.y) / gridSize)] = room;
                        break;
                    case ZoneType.LightContainment:
                        WebSiteOfFacilityManagerPlugin.LC.Rooms[(int)Math.Round((room.Position.x - WebSiteOfFacilityManagerPlugin.LC.Minimal.x) / gridSize), (int)Math.Round((room.Position.z - WebSiteOfFacilityManagerPlugin.LC.Minimal.y) / gridSize)] = room;
                        break;
                }
            }

            Log.Info("2D map created");


            for (int x = 0; x < 10; x++)
            {
                string data = "";
                for (int y = 0; y < 10; y++)
                {
                    data += Convert.ToInt32(WebSiteOfFacilityManagerPlugin.LC.Rooms[x, y]);
                }
                Log.Info(data);
            }

            Log.Info("Debug log ended");
            yield return 1; 
        }
    }
}
