using Exiled.API.Features;
using Exiled.API.Enums;

using System;
using System.Net.Sockets;


using UnityEngine;
using System.Net;
using System.Net.Http;

using MEC;
using System.Collections.Generic;
using System.IO;

namespace WebSiteOfFacilityManager
{
    public class WebSiteOfFacilityManagerPlugin : Plugin<Config>
    {
        public class Map2D
        {
            public Vector2 Minimal = new Vector2(100000, 100000);
            public Room[,] Rooms = new Room[10, 10];

            public override string ToString()
            {
                string data = "";
                try
                {
                    for (int x = 0; x < 10; x++)
                    {
                        for (int y = 0; y < 10; y++)
                        {
                            if (Rooms[x, y] != null)
                            { 
                                data += (int)Rooms[x, y].Type + ":" + (int)Math.Round(Rooms[x, y].Transform.rotation.eulerAngles.y / 90);
                            }
                            else 
                            {
                                data += "0";
                            }

                            if (y < 9)
                            {
                                data += " ";
                            }
                            else
                            {
                                data += "\n";
                            }
                        }
                    }
                } catch (Exception ex) 
                {
                    Log.Info(ex);
                }


                return data;
            }
        }

        #region Info
        public override string Name { get; } = "CubeRuben";
        public override string Prefix { get; } = "WebSiteFM";
        public override string Author { get; } = "CubeRuben";
        public override PluginPriority Priority { get; } = PluginPriority.Default;
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(2, 1, 0);
        #endregion

        EventHandlers EventHandlers;

        public static Map2D LC = new Map2D();
        public static Map2D HC = new Map2D();
        public static Map2D EZ = new Map2D();

        static Dictionary<string, string> UrlToWebSiteData = new Dictionary<string, string>();

        static HttpListener HttpServer;
        static string Address;

        public WebSiteOfFacilityManagerPlugin() 
        {

        }

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);

            Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;

            Log.Info("Starting http server");

            Address = "http://93.157.16.114:" + (Server.Port + 1000) + "/";

            HttpServer = new HttpListener();
            HttpServer.Prefixes.Add(Address);
            HttpServer.Start();

            Log.Info("Http server started on address: " + Address);

            Timing.RunCoroutine(ReadWebSiteData());

            HttpServer.BeginGetContext(new AsyncCallback(ListenerCallback), HttpServer);
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
        }

        IEnumerator<float> ReadWebSiteData() 
        {
            UrlToWebSiteData.Add(Address, File.ReadAllText(Config.WebSiteDataPath + "index.html"));
            UrlToWebSiteData.Add(Address + "main.css", File.ReadAllText(Config.WebSiteDataPath + "main.css"));
            UrlToWebSiteData.Add(Address + "main.js", File.ReadAllText(Config.WebSiteDataPath + "main.js"));

            yield return 1;
        }



        public static void ListenerCallback(IAsyncResult result)
        {
            HttpListenerContext context = HttpServer.EndGetContext(result);

            HttpServer.BeginGetContext(new AsyncCallback(ListenerCallback), HttpServer);

            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            string responseString = "";

            byte[] buffer = new byte[0];

            Log.Info(request.Url.ToString());

            string url = request.Url.ToString();

            if (UrlToWebSiteData.TryGetValue(url, out string data))
            {
                responseString = data;
            }
            else 
            {
                if (url == Address + "lczGrid")
                {
                    responseString = LC.ToString();
                }
                else if (url == Address + "hczGrid")
                {
                    responseString = HC.ToString();
                }
                else if (url == Address + "ezGrid")
                {
                    responseString = EZ.ToString();
                }
                else if (url.StartsWith(Address + "roomsImages/") && !url.Contains("..")) 
                {
                    try
                    {
                        buffer = File.ReadAllBytes(Config.WebSiteDataPath + url.Substring(Address.Length, url.Length - Address.Length));
                    }
                    catch (Exception ex) 
                    {
                        Log.Info("Unable load image: " + ex);
                    }
                } 
                else
                {
                    Log.Info("Requested not registred file");
                }         
            }

            if (buffer.Length == 0) 
            {
                buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            }
            
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }
    }
}
