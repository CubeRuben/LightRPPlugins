using Exiled.API.Features;
using Exiled.Events.EventArgs;

using UnityEngine;

using System;
using System.Runtime.Remoting.Channels;

namespace SCP173Gate
{
    class EventHandlers
    {
        SCP173GatePlugin Plugin;

        public EventHandlers(SCP173GatePlugin plugin) 
        {
            Plugin = plugin;
        }

        public void OnSendingRemoteAdminCommand(SendingRemoteAdminCommandEventArgs ev) 
        {
            if (ev.Name == "gate173") 
            {
                ev.IsAllowed = false;

                if (!Plugin.SCP173Gate) 
                {
                    ev.ReplyMessage = "Gate wasn't found";
                    return;
                }

                if (ev.Arguments.Count >= 1)
                {
                    switch (ev.Arguments[0]) 
                    {
                        case "open":
                            switch (Plugin.SCP173Gate.status) 
                            {
                                case Door.DoorStatus.Closed:
                                    Plugin.SCP173Gate.ChangeState(true);
                                    ev.ReplyMessage = "Opening the gate";
                                    break;
                                case Door.DoorStatus.Open:
                                    ev.ReplyMessage = "Gate is already open";
                                    break;
                                case Door.DoorStatus.Moving:
                                    ev.ReplyMessage = "Gate is moving";
                                    break;
                                case Door.DoorStatus.Locked:

                                    if (Plugin.SCP173Gate.moving.moving) 
                                    {
                                        ev.ReplyMessage = "Gate is moving, Gate locked";
                                        break;
                                    }

                                    if (Plugin.SCP173Gate.isOpen)
                                    {
                                        ev.ReplyMessage = "Gate is already open, Gate locked";
                                    }
                                    else 
                                    {
                                        Plugin.SCP173Gate.ChangeState(true);
                                        ev.ReplyMessage = "Opening the gate, Gate locked";
                                    }

                                    break;
                                default:
                                    ev.ReplyMessage = "Unknown status";
                                    break;
                            }
                            break;
                        case "close":
                            switch (Plugin.SCP173Gate.status)
                            {
                                case Door.DoorStatus.Closed:
                                    ev.ReplyMessage = "Gate is already closed";
                                    break;
                                case Door.DoorStatus.Open:
                                    Plugin.SCP173Gate.ChangeState(true);
                                    ev.ReplyMessage = "Closing the gate";
                                    break;
                                case Door.DoorStatus.Moving:
                                    ev.ReplyMessage = "Gate is moving";
                                    break;
                                case Door.DoorStatus.Locked:

                                    if (Plugin.SCP173Gate.moving.moving)
                                    {
                                        ev.ReplyMessage = "Gate is moving, Gate locked";
                                        break;
                                    }

                                    if (Plugin.SCP173Gate.isOpen)
                                    {
                                        Plugin.SCP173Gate.ChangeState(true);
                                        ev.ReplyMessage = "Closing the gate, Gate locked";
                                    }
                                    else
                                    { 
                                        ev.ReplyMessage = "Gate is already closed, Gate locked";
                                    }

                                    break;
                                default:
                                    ev.ReplyMessage = "Unknown status";
                                    break;
                            }
                            break;
                        case "lock":
                            if (Plugin.SCP173Gate.locked)
                            {
                                ev.ReplyMessage = "Gate is already locked";
                            }
                            else 
                            {
                                Plugin.SCP173Gate.locked = true;
                                ev.ReplyMessage = "Gate has been locked";
                            }
                            break;
                        case "unlock":
                            if (Plugin.SCP173Gate.locked)
                            {
                                Plugin.SCP173Gate.locked = false;
                                ev.ReplyMessage = "Gate has been unlocked";
                            }
                            else
                            {
                                ev.ReplyMessage = "Gate is already unlocked";
                            }
                            break;
                    }
                    
                }
                else 
                {
                    ev.ReplyMessage = "Set gate status (open, close, lock, unlock)";
                }
            }
        }

        public void OnWaitingForPlayers() 
        {
            for (int i = 0; i < Map.Doors.Count; i++) 
            {
                if ((Map.Doors[i].gameObject.transform.position.y > 14) && (Map.Doors[i].gameObject.transform.position.y < 26) && (Map.Doors[i].doorType == Door.DoorTypes.HeavyGate)) 
                {
                    Plugin.SCP173Gate = Map.Doors[i];
                    return;
                }
            }
        }

        public void OnRoundEnded(RoundEndedEventArgs ev) 
        {
            Plugin.SCP173Gate = null;
        }
    }
}
