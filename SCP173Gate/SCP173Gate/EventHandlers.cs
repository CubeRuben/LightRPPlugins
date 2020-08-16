using Exiled.API.Features;
using Exiled.Events.EventArgs;

using UnityEngine;
using UnityEngine.Networking;

using System;
using System.Runtime.Remoting.Channels;
using Mirror;
using System.Security.AccessControl;

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
            switch (ev.Name)
            { 
                case "gate173":
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
                    break;
                case "getpos":
                    ev.IsAllowed = false;
                    ev.ReplyMessage = $"{ev.Sender.Position} {Plugin.SCP173HallWayCameraPosition}";
                    break;
                    //Dubug things
                    /*
                case "getpos":
                    ev.ReplyMessage = $"{ev.Sender.Position} {Plugin.SCP173HallWayCameraPosition}";
                    break;
                case "getlayermask":
                    RaycastHit hit;
                    if (Physics.Linecast(ev.Sender.CameraTransform.position + ev.Sender.CameraTransform.forward, ev.Sender.CameraTransform.forward * 1000, out hit))
                    {
                        ev.ReplyMessage = hit.collider.gameObject.layer.ToString();
                    }
                    break;*/
            }
        }

        public void OnConsoleCommand(SendingConsoleCommandEventArgs ev) 
        {
            switch (ev.Name) 
            {
                case "enterpost":
                    ev.Allow = false;
                    if ((ev.Player.Position.x >= (Plugin.SCP173HallWayCameraPosition.x - 1)) && (ev.Player.Position.x <= (Plugin.SCP173HallWayCameraPosition.x + 1)) &&
                        (ev.Player.Position.z >= (Plugin.SCP173HallWayCameraPosition.z - 1)) && (ev.Player.Position.z <= (Plugin.SCP173HallWayCameraPosition.z + 1)) &&
                        (ev.Player.Position.y >= 16) && (ev.Player.Position.y <= 18))
                    {
                        ev.Player.Position = Plugin.SCP173ObservationPostEnterPosition;
                        ev.ReturnMessage = "You enter the observation post";
                    }
                    else 
                    {
                        ev.ReturnMessage = "Get closer to the door";
                    }
                    break;
                case "exitpost":
                    ev.Allow = false;
                    if ((ev.Player.Position.x >= (Plugin.SCP173ObservationPostEnterPosition.x - 1)) && (ev.Player.Position.x <= (Plugin.SCP173ObservationPostEnterPosition.x + 1)) &&
                        (ev.Player.Position.z >= (Plugin.SCP173ObservationPostEnterPosition.z - 1)) && (ev.Player.Position.z <= (Plugin.SCP173ObservationPostEnterPosition.z + 1)) &&
                        (ev.Player.Position.y >= 20) && (ev.Player.Position.y <= 22))
                    {
                        ev.Player.Position = Plugin.SCP173HallWayCameraPosition;
                        ev.ReturnMessage = "You leave the observation post";
                    }
                    else
                    {
                        ev.ReturnMessage = "Get closer to the exit of observation post";
                    }
                    break;
            }
        }

        public void OnWaitingForPlayers() 
        {

            for (int i = 0; i < Map.Cameras.Length; i++)
            {
                if (Map.Cameras[i].cameraName == "173 HALLWAY") 
                {
                    Plugin.SCP173HallWayCameraPosition = Map.Cameras[i].transform.position;
                    Plugin.SCP173HallWayCameraPosition.y = 17.1f;
                    break;
                }
            }

            for (int i = 0; i < Map.Doors.Count; i++) 
            {
                if (Plugin.SCP173ObservationPostEnterPosition == Vector3.zero) 
                {
                    if (Map.Doors[i].DoorName == "173") 
                    {
                        Plugin.SCP173ObservationPostEnterPosition = Map.Doors[i].transform.position + (Map.Doors[i].transform.right * -14.6f) + (Map.Doors[i].transform.forward * -4.25f);
                        Plugin.SCP173ObservationPostEnterPosition.y = 21.0f;


                        //Спавн кубов, чтобы не вылазить за пределы поста обзора 
                        {
                            GameObject barrier = GameObject.CreatePrimitive(PrimitiveType.Cube);

                            Vector3 pos = Map.Doors[i].transform.position + (Map.Doors[i].transform.right * -13.05f);
                            pos.y = 20.0f;
                            barrier.transform.position = pos;

                            barrier.layer = 0;

                            barrier.transform.localScale = Map.Doors[i].transform.right * 16.5f + Map.Doors[i].transform.forward + new Vector3(0, 10, 0);

                            Plugin.Barriers[0] = barrier;
                        }

                        {
                            GameObject barrier = GameObject.CreatePrimitive(PrimitiveType.Cube);

                            Vector3 pos = Map.Doors[i].transform.position + (Map.Doors[i].transform.forward * -2.6f) + (Map.Doors[i].transform.right * -5.4f);
                            pos.y = 20.0f;
                            barrier.transform.position = pos;

                            barrier.layer = 0;

                            barrier.transform.localScale = Map.Doors[i].transform.right + Map.Doors[i].transform.forward * -4.2f + new Vector3(0, 10, 0);

                            Plugin.Barriers[1] = barrier;

                            //Этот куб нужен для того, чтобы не выкидывало за стенку
                            GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);

                            block.transform.position = barrier.transform.position + Map.Doors[i].transform.right * 4 + new Vector3(0, 4);

                            block.transform.localScale = new Vector3(6, 5, 6);
                        }


                        if (Plugin.SCP173Gate) 
                        {
                            break;
                        }
                    }
                }

                if (!Plugin.SCP173Gate)
                {
                    if ((Map.Doors[i].gameObject.transform.position.y > 14) && (Map.Doors[i].gameObject.transform.position.y < 26) && (Map.Doors[i].doorType == Door.DoorTypes.HeavyGate))
                    {
                        Plugin.SCP173Gate = Map.Doors[i];

                        if (Plugin.SCP173ObservationPostEnterPosition != Vector3.zero)
                        {
                            break;
                        }
                    }
                }
            }
        }

        public void OnRoundEnded(RoundEndedEventArgs ev) 
        {
            Plugin.SCP173Gate = null;
            Plugin.SCP173ObservationPostEnterPosition = Vector3.zero;
            Plugin.SCP173HallWayCameraPosition = Vector3.zero;

        }

        public void OnHurting(HurtingEventArgs ev) 
        {
            if ((ev.HitInformations.Attacker == "*You have exceeded the teleportation limit and we couldn't find a safe position near your location.\n(debug code: A.3)") && (ev.HitInformations.Tool == DamageTypes.ToIndex(DamageTypes.Flying)))
            {
                if (Vector3.Distance(Plugin.Barriers[0].transform.position, ev.Target.Position) <= 8)
                {
                    ev.IsAllowed = false;
                }
                else if (Vector3.Distance(Plugin.Barriers[1].transform.position, ev.Target.Position) <= 4)
                {
                    ev.IsAllowed = false;
                }
            }
        }
    }
}
