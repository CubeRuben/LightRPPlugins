using Exiled.Events.EventArgs;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;

using System;
using System.Linq;

namespace BanWebhook
{
    public class EventHandlers
    {

        BanWebhookPlugin Plugin;

        public EventHandlers(BanWebhookPlugin plugin) 
        {
            Plugin = plugin;
        }

        string GetSteamLinkFromToken(string authToken) 
        {
            string id = authToken.Split('<')[0].Split(' ')[2];

            if (id.EndsWith("@steam"))
            {
                return GetSteamLink(id.Replace("@steam", string.Empty));
            }

            return "";
        }

        string GetSteamLink(string id) 
        {
            return $"https://steamcommunity.com/profiles/{id}/";
        }

        Player GetPlayerByNickname(string nickname) 
        {
            foreach (Player player in Player.Dictionary.Values.ToArray())
            {
                if (player.Nickname == nickname)
                {
                    return player;
                }
            }

            return null;
        }

        public void OnBanned(BannedEventArgs ev)
        {
            if (ev.Type != BanHandler.BanType.UserId) 
            {
                return;
            }

            if (ev == null) 
            {
                return;
            }

            if (ev.Player == null)
            {
                return;
            }

            Player admin = GetPlayerByNickname(ev.Details.Issuer);

            if (admin != null)
            {
                if (ev.Player.Id != admin.Id)
                {
                    Plugin.AddBanCount(admin.UserId);
                }
            }

            string message;

            if (admin == null)
            {
                message = "{\"embeds\":[{\"title\":\"Игрок забанен\",\"fields\":[{\"name\":\"Нарушитель\",\"value\":\"[" + ev.Player.Nickname + "](" + GetSteamLinkFromToken(ev.Player.AuthenticationToken) + ")\",\"inline\":\"true\"},{\"name\":\"Причина\",\"value\":\"" + (ev.Details.Reason == "" ? "Не указано" : ev.Details.Reason) + "\"},{\"name\":\"Дата и время бана\",\"value\":\"" + DateTime.UtcNow.AddHours(3).ToString("dd.MM.yy HH:mm") + " (МСК)\",\"inline\":\"true\"},{\"name\":\"Дата и время окончания бана\",\"value\":\"" + DateTime.FromBinary(ev.Details.Expires).AddHours(3).ToString("dd.MM.yy HH:mm") + " (МСК)\",\"inline\":\"true\"}]}]}";
            }
            else
            {
                message = "{\"embeds\":[{\"title\":\"Игрок забанен\",\"fields\":[{\"name\":\"Администратор\",\"value\":\"[" + admin.Nickname + "](" + GetSteamLinkFromToken(admin.AuthenticationToken) + ")\",\"inline\":\"true\"},{\"name\":\"Нарушитель\",\"value\":\"[" + ev.Player.Nickname + "](" + GetSteamLinkFromToken(ev.Player.AuthenticationToken) + ")\",\"inline\":\"true\"},{\"name\":\"Причина\",\"value\":\"" + (ev.Details.Reason == "" ? "Не указано" : ev.Details.Reason) + "\"},{\"name\":\"Дата и время бана\",\"value\":\"" + DateTime.UtcNow.AddHours(3).ToString("dd.MM.yy HH:mm") + " (МСК)\",\"inline\":\"true\"},{\"name\":\"Дата и время окончания бана\",\"value\":\"" + DateTime.FromBinary(ev.Details.Expires).AddHours(3).ToString("dd.MM.yy HH:mm") + " (МСК)\",\"inline\":\"true\"}]}]}";
            }

            Timing.RunCoroutine(Plugin.SendWeebhook(message));
        }

        public void OnOfflineBan(string steamid, int time, string reason, Player admin) 
        {
            string message;

            if (admin == null)
            {
                message = "{\"embeds\":[{\"title\":\"Игрок забанен с помощью OfflineBan\",\"fields\":[{\"name\":\"Нарушитель\",\"value\":\"[Игрок](" + GetSteamLink(steamid) + ")\",\"inline\":\"true\"},{\"name\":\"Причина\",\"value\":\"" + (reason == "" ? "Не указано" : reason) + "\"},{\"name\":\"Дата и время бана\",\"value\":\"" + DateTime.UtcNow.AddHours(3).ToString("dd.MM.yy HH:mm") + " (МСК)\",\"inline\":\"true\"},{\"name\":\"Дата и время окончания бана\",\"value\":\"" + DateTime.UtcNow.AddHours(3).AddMinutes(time).ToString("dd.MM.yy HH:mm") + " (МСК)\",\"inline\":\"true\"}]}]}";
            }
            else
            {
                message = "{\"embeds\":[{\"title\":\"Игрок забанен с помощью OfflineBan\",\"fields\":[{\"name\":\"Администратор\",\"value\":\"[" + admin.Nickname + "](" + GetSteamLinkFromToken(admin.AuthenticationToken) + ")\",\"inline\":\"true\"},{\"name\":\"Нарушитель\",\"value\":\"[Игрок](" + GetSteamLink(steamid) + ")\",\"inline\":\"true\"},{\"name\":\"Причина\",\"value\":\"" + (reason == "" ? "Не указано" : reason) + "\"},{\"name\":\"Дата и время бана\",\"value\":\"" + DateTime.UtcNow.AddHours(3).ToString("dd.MM.yy HH:mm") + " (МСК)\",\"inline\":\"true\"},{\"name\":\"Дата и время окончания бана\",\"value\":\"" + DateTime.UtcNow.AddHours(3).AddMinutes(time).ToString("dd.MM.yy HH:mm") + " (МСК)\",\"inline\":\"true\"}]}]}";
            }

            Timing.RunCoroutine(Plugin.SendWeebhook(message));
        }
        
        public void OnKicking(KickingEventArgs ev) 
        {
            if (ev.Target == null) 
            {
                return;
            }

            if (ev.Reason.StartsWith("You have been banned.")) 
            {
                return;
            }

            string message;

            if (ev.Issuer == null)
            {
                message = "{\"embeds\":[{\"title\":\"Игрок кикнут\",\"fields\":[{\"name\":\"Нарушитель\",\"value\":\"[" + ev.Target.Nickname + "](" + GetSteamLinkFromToken(ev.Target.AuthenticationToken) + ")\",\"inline\":\"true\"},{\"name\":\"Причина\",\"value\":\"" + (ev.Reason == "" ? "Не указано" : ev.Reason) + "\"},{\"name\":\"Дата и время кика\",\"value\":\"" + DateTime.UtcNow.AddHours(3).ToString("dd.MM.yy HH:mm") + " (МСК)\",\"inline\":\"true\"}]}]}";
            }
            else
            {
                message = "{\"embeds\":[{\"title\":\"Игрок кикнут\",\"fields\":[{\"name\":\"Администратор\",\"value\":\"[" + ev.Issuer.Nickname + "](" + GetSteamLinkFromToken(ev.Issuer.AuthenticationToken) + ")\",\"inline\":\"true\"},{\"name\":\"Нарушитель\",\"value\":\"[" + ev.Target.Nickname + "](" + GetSteamLinkFromToken(ev.Target.AuthenticationToken) + ")\",\"inline\":\"true\"},{\"name\":\"Причина\",\"value\":\"" + (ev.Reason == "" ? "Не указано" : ev.Reason) + "\"},{\"name\":\"Дата и время кика\",\"value\":\"" + DateTime.UtcNow.AddHours(3).ToString("dd.MM.yy HH:mm") + " (МСК)\",\"inline\":\"true\"}]}]}";
            }

            Timing.RunCoroutine(Plugin.SendWeebhook(message));
        }
    }
}
