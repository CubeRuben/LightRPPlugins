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
            Exiled.Events.Handlers.Player.Kicking += EventHandlers.OnKicking;
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.Banned -= EventHandlers.OnBanned;
            Exiled.Events.Handlers.Player.Kicking -= EventHandlers.OnKicking;

            EventHandlers = null;
        }

        public void AddBanCount(string id) 
        {
            string file = File.ReadAllText(Config.PathOfAdminBansCount);

            string[] data = file.Split('\n');

            for (int i = 0; i < data.Length; i++) 
            {
                string[] playerData = data[i].Split(':');
                if (playerData[0] == id) 
                {
                    int count = int.Parse(playerData[1]);
                    count++;

                    string final = "";
                    for (int a = 0; a < data.Length; a++)
                    {
                        if (i != a)
                        {
                            final += data[a];
                        }
                        else 
                        {
                            final += playerData[0] + ":" + count.ToString();
                        }

                        if (a + 1 != data.Length) 
                        {
                            final += "\n";
                        }
                    }

                    File.WriteAllText(Config.PathOfAdminBansCount, final);
                    return;
                }
            }

            file += "\n" + id + ":1";

            File.WriteAllText(Config.PathOfAdminBansCount, file);
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
