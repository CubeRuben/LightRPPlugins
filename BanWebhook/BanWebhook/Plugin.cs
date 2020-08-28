using Exiled.API.Features;
using Exiled.API.Enums;

using System;
using System.Net;
using System.Net.Http;
using System.Collections.Specialized;
using System.IO;
using System.Text;

using MEC;
using System.Collections.Generic;

namespace BanWebhook
{
    public class BanWebhookPlugin : Plugin<Config>
    {
        #region Info
        public override string Name { get; } = "Ban Webhook Plugin";
        public override string Prefix { get; } = "BanWeb";
        public override string Author { get; } = "CubeRuben";
        public override PluginPriority Priority { get; } = PluginPriority.Default;
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(2, 1, 0);
        #endregion

        EventHandlers EventHandlers;

        public BanWebhookPlugin() 
        {
        }

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);

            Exiled.Events.Handlers.Player.Banned += EventHandlers.OnBanned;
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.Banned -= EventHandlers.OnBanned;

            EventHandlers = null;
        }

        public IEnumerator<float> SendWeebhook(string message)
        { 
            WebRequest request = WebRequest.Create(Config.WebHookURL);

            byte[] byteArray = Encoding.UTF8.GetBytes(message);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            yield return 1;
        }
    }
}
