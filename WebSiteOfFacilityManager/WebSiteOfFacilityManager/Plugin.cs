using Exiled.API.Features;
using Exiled.API.Enums;

using System;
using System.Net.Sockets;


using UnityEngine;
using System.Net;

namespace WebSiteOfFacilityManager
{
    public class WebSiteOfFacilityManagerPlugin : Plugin<Config>
    {
        public class Map2D 
        {
            public Vector2 Minimal = new Vector2(100000, 100000);
            public Room[,] Rooms = new Room[10, 10];
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

        public Map2D LC = new Map2D();
        public Map2D HC = new Map2D();
        public Map2D EZ = new Map2D();

        Socket WebServerSocket;

        public WebSiteOfFacilityManagerPlugin() 
        {
            WebServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            WebServerSocket.Bind(new IPEndPoint(long.Parse(Server.Host.IPAddress), Server.Port));
        }

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);

            Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
        }
    }
}
