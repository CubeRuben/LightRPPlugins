using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System;
using UnityEngine;
using MEC;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace LightSystem
{
    class EventHandlers
    {
        LightSystemPlugin Plugin;

        public EventHandlers(LightSystemPlugin plugin) 
        {
            Plugin = plugin;
        }

        public void SwitchLight(List<FlickerableLightController> array, bool lightState, float time) 
        {
            for (int i = 0; i < array.Count; i++) 
            {
                SwitchLight(array[i], lightState, time);
            }
        }

        public void SwitchLight(FlickerableLightController component, bool lightState, float time) 
        {
            if (time == 0)
            {
                if (lightState)
                {
                    component.ServerFlickerLights(0);
                }
                else
                {
                    component.ServerFlickerLights(float.MaxValue);
                }
            }
            else 
            {
                component.ServerFlickerLights(time);
            }
        }

        public void OnSendingRemoteAdminCommand(SendingRemoteAdminCommandEventArgs ev) 
        {
            switch (ev.Name)
            {
                case "lights":
                    ev.IsAllowed = false;

                    if (ev.Arguments.Count < 1)
                    {
                        ev.ReplyMessage = "No light state or time";
                        break;
                    }

                    if (ev.Arguments.Count < 2)
                    {
                        ev.ReplyMessage = "No zone type or room";
                        break;
                    }

                    float time = 0;

                    if (!bool.TryParse(ev.Arguments[0], out bool lightState) && !float.TryParse(ev.Arguments[0], out time))
                    {
                        ev.ReplyMessage = "Uncorrect ligth state or time";
                        break;
                    }

                    switch (ev.Arguments[1]) 
                    {
                        case "room":
                            Player player = null;

                            if (ev.Arguments.Count < 4) 
                            {
                                player = Player.Get(ev.Arguments[2]);
                            }

                            if (player == null) 
                            {
                                player = ev.Sender;
                            }

                            if (player.CurrentRoom == null) 
                            {
                                ev.ReplyMessage = "Player not in room";
                                break;
                            }

                            FlickerableLightController component = player.CurrentRoom.Transform.gameObject.GetComponentInChildren<FlickerableLightController>();

                            if (component == null) 
                            {
                                ev.ReplyMessage = "Can't flick light";
                                break;
                            }

                            SwitchLight(component, lightState, time);
                            ev.ReplyMessage = "Done";
                            break;
                        case "lcz":
                            SwitchLight(Plugin.LCZ, lightState, time);
                            break;
                        case "hcz":
                            SwitchLight(Plugin.HCZ, lightState, time);
                            break;
                        case "adm":
                        case "ez":
                            SwitchLight(Plugin.EZ, lightState, time);
                            break;
                    }
                    break;
            }
        }

        public void OnWaitingForPlayers() 
        {
            Plugin.EZ.Clear();
            Plugin.HCZ.Clear();
            Plugin.LCZ.Clear();

            for (int i = 0; i < Map.Rooms.Count; i++)
            {
                FlickerableLightController component = Map.Rooms[i].Transform.gameObject.GetComponentInChildren<FlickerableLightController>();
                switch (Map.Rooms[i].Zone)
                {
                    case ZoneType.Entrance:
                        Plugin.EZ.Add(component);
                        break;
                    case ZoneType.HeavyContainment:
                        Plugin.HCZ.Add(component);
                        break;
                    case ZoneType.LightContainment:
                        Plugin.LCZ.Add(component);
                        break;
                }
            }
        }
    }
}
