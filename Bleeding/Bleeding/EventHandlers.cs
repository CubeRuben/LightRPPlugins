using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using System.Threading;
using UnityEngine.Assertions.Must;
using Exiled.Events.Commands.Reload;

namespace Bleeding
{
    class EventHandlers
    {
        BleedingPlugin Plugin;

        public EventHandlers(BleedingPlugin plugin) 
        {
            Plugin = plugin;
        }

        public void OnSendingRemoteAdminCommand(SendingRemoteAdminCommandEventArgs ev) 
        {
            switch (ev.Name) 
            {
                case "bleedout":
                case "bo":
                    {
                        ev.IsAllowed = false;

                        if (ev.Arguments.Count < 1)
                        {
                            ev.ReplyMessage = "No player id";
                            break;
                        }

                        if (ev.Arguments.Count < 2)
                        {
                            ev.ReplyMessage = "No damage value";
                            break;
                        }

                        if (ev.Arguments.Count < 3)
                        {
                            ev.ReplyMessage = "No time value";
                            break;
                        }

                        if (ev.Arguments.Count < 4)
                        {
                            ev.ReplyMessage = "No damage interval value";
                            break;
                        }

                        if (ev.Arguments.Count < 5)
                        {
                            ev.ReplyMessage = "No blood size";
                            break;
                        }

                        if (ev.Arguments.Count < 6)
                        {
                            ev.ReplyMessage = "No blood spawn rate";
                            break;
                        }

                        if (!int.TryParse(ev.Arguments[0], out int playerId))
                        {
                            ev.ReplyMessage = "Uncorrect player id";
                            break;
                        }

                        if (!float.TryParse(ev.Arguments[1], out float damage))
                        {
                            ev.ReplyMessage = "Uncorrect damage";
                            break;
                        }

                        if (!float.TryParse(ev.Arguments[2], out float time))
                        {
                            ev.ReplyMessage = "Uncorrect time";
                            break;
                        }

                        if (!float.TryParse(ev.Arguments[3], out float interval))
                        {
                            ev.ReplyMessage = "Uncorrect interval";
                            break;
                        }

                        if (!float.TryParse(ev.Arguments[4], out float bloodSize))
                        {
                            ev.ReplyMessage = "Uncorrect blood size";
                            break;
                        }

                        if (!float.TryParse(ev.Arguments[5], out float bloodSpawnRate))
                        {
                            ev.ReplyMessage = "Uncorrect bloodSpawnRate";
                            break;
                        }

                        if (!Plugin.PlayersHealth.ContainsKey(playerId))
                        {
                            ev.ReplyMessage = "No player with this id";
                            break;
                        }

                        Plugin.PlayersHealth[playerId].AddBleeding(PlayerHealth.BleedingTypes.Custom, 100, damage, time, interval, bloodSize, bloodSpawnRate);
                    }
                    break;
                case "ab":
                case "addbleeding":
                    {
                        ev.IsAllowed = false;

                        if (ev.Arguments.Count < 1) 
                        {
                            ev.ReplyMessage = "No player id";
                            break;
                        }

                        if (ev.Arguments.Count < 2)
                        {
                            ev.ReplyMessage = "No bleeding type (Low \\ 1, Medium \\ 2, High \\ 3, Arterial \\ 4)";
                            break;
                        }

                        if (!int.TryParse(ev.Arguments[0], out int playerId)) 
                        {
                            ev.ReplyMessage = "Uncorrect player id";
                            break;
                        }

                        if (!Enum.TryParse(ev.Arguments[1], out PlayerHealth.BleedingTypes bleedingType) || (bleedingType == PlayerHealth.BleedingTypes.None)) 
                        {
                            ev.ReplyMessage = "Uncorrect bleeding type";
                            break;  
                        }


                        if (!Plugin.PlayersHealth.ContainsKey(playerId))
                        {
                            ev.ReplyMessage = "No player with this id";
                            break;    
                        }

                        Plugin.PlayersHealth[playerId].AddBleeding(bleedingType);
                    }
                    break;
            }
        }

        public void OnSendingConsoleCommand(SendingConsoleCommandEventArgs ev) 
        {
            switch (ev.Name) 
            {
                case "stopblood":
                    ev.IsAllowed = false;

                    if ((ev.Player.Role == RoleType.Scp93989) || (ev.Player.Role == RoleType.Scp93953)) 
                    {
                        ev.ReturnMessage = "You cannot stop blood";
                        break;
                    }

                    if ((ev.Player.Team == Team.SCP) && (ev.Player.Role != RoleType.Scp049)) 
                    {
                        ev.ReturnMessage = "You cannot have bleeding";
                        break;
                    }

                    PlayerHealth playerHealth = Plugin.PlayersHealth[ev.Player.Id];

                    if (playerHealth.CurrentHighestBleedingType == PlayerHealth.BleedingTypes.None) 
                    {
                        ev.ReturnMessage = "You are not bleeding";
                        break;
                    }

                    if ((playerHealth.StopBloodStatus == PlayerHealth.StopBlood.Stoping) || (Plugin.PlayersHealth[ev.Player.Id].CommandStopBloodCoroutineHandle != null)) 
                    {
                        ev.ReturnMessage = "You already stopped the bleeding";
                        break;
                    }

                    Plugin.PlayersHealth[ev.Player.Id].CommandStopBloodCoroutineHandle = Timing.RunCoroutine(BloodStoppingCoroutine(Plugin.PlayersHealth[ev.Player.Id]));
                    ev.ReturnMessage = "You start stop blood";
                    break;
            }
        }

        IEnumerator<float> BloodStoppingCoroutine(PlayerHealth playerHealth, float time = 10) 
        {
            playerHealth.Player.Inventory.SetCurItem(ItemType.None);
            playerHealth.StopBloodStatus = PlayerHealth.StopBlood.Stoping;
            Vector3 startPos = playerHealth.Player.Position;

            for (float timer = 0; (timer < time) && (playerHealth.StopBloodStatus == PlayerHealth.StopBlood.Stoping); timer += 0.25f) 
            {
                playerHealth.Player.ClearBroadcasts();
                playerHealth.Player.Broadcast(1, $"Вы останавливаете кровь");
                yield return Timing.WaitForSeconds(0.25f);
                if (startPos != playerHealth.Player.Position)
                {
                    playerHealth.StopBloodStatus = PlayerHealth.StopBlood.Failed;
                    break;
                }

                if (playerHealth.CurrentHighestBleedingType == PlayerHealth.BleedingTypes.None)
                {
                    playerHealth.StopBloodStatus = PlayerHealth.StopBlood.None;
                    break;
                }
            }

            if (playerHealth.StopBloodStatus == PlayerHealth.StopBlood.Stoping)
            {
                playerHealth.CmdStopBlood();
            }
            else if (playerHealth.StopBloodStatus == PlayerHealth.StopBlood.Failed)
            {
                playerHealth.StopBloodStatus = PlayerHealth.StopBlood.None;
                playerHealth.Player.ClearBroadcasts();
                playerHealth.Player.Broadcast(5, "Вы перестали останавливать кровь");
                playerHealth.UpdateBleedingBroadcast();
            }
            

            playerHealth.CommandStopBloodCoroutineHandle = null;
        }

        public void OnJoined(JoinedEventArgs ev) 
        {
            Plugin.PlayersHealth.Add(ev.Player.Id, new PlayerHealth(ev.Player));
        }

        public void OnLeft(LeftEventArgs ev) 
        {
            Plugin.PlayersHealth[ev.Player.Id].Clear();
            Plugin.PlayersHealth.Remove(ev.Player.Id);
        }

        public void OnHurting(HurtingEventArgs ev) 
        {
            if ((ev.DamageType != DamageTypes.Bleeding) && (ev.DamageType != DamageTypes.Scp207))
            {
                FailStopBlood(ev.Target.Id);
            }
            Plugin.PlayersHealth[ev.Target.Id].OnHurting(ev.DamageType);
        }

        public void OnDied(DiedEventArgs ev)
        {
            Plugin.PlayersHealth[ev.Target.Id].Clear();
        }

        public void OnChangingRole(ChangingRoleEventArgs ev) 
        {
            Plugin.PlayersHealth[ev.Player.Id].Clear();
        }

        public void OnMedicalItemUsed(UsedMedicalItemEventArgs ev) 
        {
            Plugin.PlayersHealth[ev.Player.Id].OnHeal(ev.Item);
        }

        public void OnInteracted(InteractedEventArgs ev) 
        {
            FailStopBlood(ev.Player.Id);
        }

        public void OnItemDropped(ItemDroppedEventArgs ev) 
        {
            FailStopBlood(ev.Player.Id);
        }

        public void OnChangingItem(ChangingItemEventArgs ev) 
        {
            FailStopBlood(ev.Player.Id);
        }

        public void OnHandcuffing(HandcuffingEventArgs ev) 
        {
            FailStopBlood(ev.Target.Id);
        }

        public void OnPickingUpItem(PickingUpItemEventArgs ev) 
        {
            FailStopBlood(ev.Player.Id);
        }

        void FailStopBlood(int id) 
        {
            Plugin.PlayersHealth[id].FailStopBlood();
        }
    }
}
