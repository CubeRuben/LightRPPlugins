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
			if (ev.get_Name().ToLower() != "atban")
			{
				return;
			}
			ev.set_IsAllowed(false);
			if (ev.get_Arguments().Count < 2)
			{
				ev.get_Sender().RemoteAdminMessage("Out of args. " + GetUsageAtBan(), true, (string)null);
				return;
			}
			if (!int.TryParse(ev.get_Arguments()[1], out int result))
			{
				ev.get_Sender().RemoteAdminMessage("Wrong args. " + GetUsageAtBan(), true, (string)null);
				return;
			}
			if (result <= 0)
			{
				ev.get_Sender().RemoteAdminMessage("Wrong args. " + GetUsageAtBan(), true, (string)null);
				return;
			}
			string steamid = ev.get_Arguments()[0];
			string text = string.Empty;
			if (ev.get_Arguments().Count == 3)
			{
				text = ev.get_Arguments()[2];
			}
			if ((from x in Player.get_List()
				where x.get_UserId().Replace("@steam", string.Empty) == steamid
				select x).FirstOrDefault() != null)
			{
				(from x in Player.get_List()
					where x.get_UserId().Replace("@steam", string.Empty) == steamid
					select x).FirstOrDefault().Ban(result, text, ev.get_Sender().get_Nickname());
				ev.get_Sender().RemoteAdminMessage("Success ban " + steamid, true, (string)null);
			}
			else
			{
				BanDetails ban = new BanDetails
				{
					Expires = DateTime.UtcNow.AddMinutes(result).Ticks,
					Id = steamid,
					IssuanceTime = TimeBehaviour.CurrentTimestamp(),
					Issuer = ev.get_Sender().get_Nickname(),
					Reason = text,
					OriginalName = string.Empty
				};
				BanHandler.IssueBan(ban, BanHandler.BanType.UserId);
				ev.get_Sender().RemoteAdminMessage("Success offline ban " + steamid, true, (string)null);
			}
		}
	}
}
