using Exiled.API.Features;
using Exiled.Events.EventArgs;

namespace CustomDamage
{
    class EventHandlers
    {
        CustomDamagePlugin Plugin;

        public EventHandlers(CustomDamagePlugin plugin)
        {
            Plugin = plugin;
        }

        public void OnSendingRemoteAdminCommand(SendingRemoteAdminCommandEventArgs ev) 
        {
            switch (ev.Name) 
            {
                case "scd":
                case "setdamage":
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

                        Player player;

                        if (int.TryParse(ev.Arguments[0], out int id))
                        {
                            player = Player.Get(id);
                        }
                        else
                        {
                            player = Player.Get(ev.Arguments[0]);
                        }

                        if (player == null)
                        {
                            ev.ReplyMessage = "Player not founded";
                            break;
                        }

                        if (!float.TryParse(ev.Arguments[1], out float damage))
                        {
                            ev.ReplyMessage = "Uncorrect damage value";
                            break;
                        }

                        Plugin.PlayerToCustomDamage.Remove(player.Id);
                        Plugin.PlayerToCustomDamage.Add(player.Id, damage);

                        ev.ReplyMessage = "Custom damage added";
                    }
                    break;
                case "rcd":
                case "resetcustomdamage":
                    {
                        ev.IsAllowed = false;

                        if (ev.Arguments.Count < 1)
                        {
                            ev.ReplyMessage = "No player id";
                            break;
                        }

                        Player player;

                        if (int.TryParse(ev.Arguments[0], out int id))
                        {
                            player = Player.Get(id);
                        }
                        else
                        {
                            player = Player.Get(ev.Arguments[0]);
                        }

                        if (player == null)
                        {
                            ev.ReplyMessage = "Player not founded";
                            break;
                        }

                        Plugin.PlayerToCustomDamage.Remove(player.Id);
                        ev.ReplyMessage = "Custom damage reseted";
                    }
                    break;
            }
        
        }

        public void OnWaitingForPlayers() 
        {
            Plugin.PlayerToCustomDamage.Clear();
        }

        public void OnHurting(HurtingEventArgs ev)
        {
            if (ev.Attacker == null)
            {
                return;
            }

            if (!Plugin.PlayerToCustomDamage.TryGetValue(ev.Attacker.Id, out float damage)) 
            {
                return;
            }

            ev.Amount = damage;
        }
    }
}
