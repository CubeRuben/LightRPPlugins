using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System;
using System.Linq;

namespace OfflineBans
{
    public class SetEvents
    {
        private string GetUsageAtBan()
        {
            return "Usage: atban <SteamID64> <Time> <Reason>";
        }

        internal void OnSendingRemoteAdminCommand(SendingRemoteAdminCommandEventArgs ev)
        {
            if (ev.Name.ToLower() != "atban")
                return;

            ev.IsAllowed = false;
            if (ev.Arguments.Count < 2)
            {
                ev.Sender.RemoteAdminMessage("Out of args. " + GetUsageAtBan());
                return;
            }
            if (!int.TryParse(ev.Arguments[1], out int time))
            {
                ev.Sender.RemoteAdminMessage("Wrong args. " + GetUsageAtBan());
                return;
            }
            if (time <= 0)
            {
                ev.Sender.RemoteAdminMessage("Wrong args. " + GetUsageAtBan());
                return;
            }
            string steamid = ev.Arguments[0];
            string reason = string.Empty;
            if (ev.Arguments.Count == 3)
                reason = ev.Arguments[2];

            if (Player.List.Where(x => x.UserId.Replace("@steam", string.Empty) == steamid).FirstOrDefault() != default)
            {
                Player.List.Where(x => x.UserId.Replace("@steam", string.Empty) == steamid).FirstOrDefault().Ban(time, reason, ev.Sender.Nickname);
                ev.Sender.RemoteAdminMessage("Success ban " + steamid);
                return;
            }
            else
            {
                BanDetails banDetails = new BanDetails()
                {
                    Expires = DateTime.UtcNow.AddMinutes(time).Ticks,
                    Id = steamid,
                    IssuanceTime = TimeBehaviour.CurrentTimestamp(),
                    Issuer = ev.Sender.Nickname,
                    Reason = reason,
                    OriginalName = string.Empty
                };
                BanHandler.IssueBan(banDetails, BanHandler.BanType.UserId);
                ev.Sender.RemoteAdminMessage("Success offline ban " + steamid);
                return;
            }
        }
    }
}