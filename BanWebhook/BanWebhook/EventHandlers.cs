using Exiled.Events.EventArgs;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;

using System;

namespace BanWebhook
{
    class EventHandlers
    {

        BanWebhookPlugin Plugin;

        public EventHandlers(BanWebhookPlugin plugin) 
        {
            Plugin = plugin;
        }

        string GetSteamLink(string authToken) 
        {
            string id = authToken.Split('<')[0].Split(' ')[2];

            if (id.EndsWith("@steam"))
            {
                return $"https://steamcommunity.com/profiles/{id.Replace("@steam", "")}/";
            }

            return "";
        }

        public void OnBanned(BannedEventArgs ev)
        {
            if (ev.Player == null) 
            {
                return;
            }

            Player admin = Player.Get(ev.Details.Issuer);

            string message = "{\"embeds\":[{\"title\":\"Игрок забанен\",\"fields\":[{\"name\":\"Администратор\",\"value\":\"[" + admin.Nickname + "](" + GetSteamLink(admin.AuthenticationToken) + ")\",\"inline\":\"true\"},{\"name\":\"Нарушитель\",\"value\":\"[" + ev.Player.Nickname + "](" + GetSteamLink(ev.Player.AuthenticationToken) + ")\",\"inline\":\"true\"},{\"name\":\"Причина\",\"value\":\"" + (ev.Details.Reason == "" ? "Не указано" : ev.Details.Reason) + "\"},{\"name\":\"Дата и время бана\",\"value\":\"" + DateTime.UtcNow.AddHours(3).ToString("dd.MM.yy HH:mm") + " (МСК)\",\"inline\":\"true\"},{\"name\":\"Дата и время окончания бана\",\"value\":\"" + DateTime.FromBinary(ev.Details.Expires).AddHours(3).ToString("dd.MM.yy HH:mm") + " (МСК)\",\"inline\":\"true\"}]}]}";

            Log.Info(message);
            
            Timing.RunCoroutine(Plugin.SendWeebhook(message));
        }
    }
}
