using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System;
using System.Windows.Forms;

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
                case "addbleeding":
                    {
                        ev.IsAllowed = false;

                        if (ev.Arguments.Count < 1) 
                        {
                            ev.ReplyMessage = "No player id";
                            break;
                        }

                        if (!int.TryParse(ev.Arguments[0], out int playerId)) 
                        {
                            ev.ReplyMessage = "Uncorrect player id";
                            break;
                        }

                        if (ev.Arguments.Count < 2) 
                        {
                            ev.ReplyMessage = "No bleeding type (Low, Medium, High, Arterial)";
                            break;
                        }

                        if (!Enum.TryParse(ev.Arguments[1], out PlayerHealth.BleedingTypes bleedingType)) 
                        {
                            ev.ReplyMessage = "Uncorrect player id";
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

        public void OnJoined(JoinedEventArgs ev) 
        {
            Plugin.PlayersHealth.Add(ev.Player.Id, new PlayerHealth(ev.Player));
        }

        public void OnLeft(LeftEventArgs ev) 
        {
            Plugin.PlayersHealth.Remove(ev.Player.Id);
        }

        public void OnHurting(HurtingEventArgs ev) 
        {
            Plugin.PlayersHealth[ev.Target.Id].OnHurting(ev.DamageType);
        }

        public void OnDied(DiedEventArgs ev)
        {
            Plugin.PlayersHealth[ev.Target.Id].Clear();
        }

        public void OnSpawning(SpawningEventArgs ev) 
        {
            Plugin.PlayersHealth[ev.Player.Id].Clear();
        }
    }
}
