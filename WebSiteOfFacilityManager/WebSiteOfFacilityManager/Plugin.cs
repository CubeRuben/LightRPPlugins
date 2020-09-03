using Exiled.API.Features;
using Exiled.API.Enums;

using System;
using System.Reflection;

using HarmonyLib;
using System.CodeDom;

namespace WebSiteOfFacilityManager
{
    public class WebSiteOfFacilityManagerPlugin : Plugin<Config>
    {
        #region Info
        public override string Name { get; } = "CubeRuben";
        public override string Prefix { get; } = "WebSiteFM";
        public override string Author { get; } = "CubeRuben";
        public override PluginPriority Priority { get; } = PluginPriority.Default;
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(2, 1, 0);
        #endregion

        EventHandlers EventHandlers;

        
        public static Room[,] LCZ = new Room[10, 10];
        public static Room[,] HCZ = new Room[10, 10];
        public static Room[,] EZ = new Room[10, 10];

        public static FieldInfo AliasOfImagaGenerator;

        Harmony Harmony;

        public readonly FieldInfo MinimapField;

        public WebSiteOfFacilityManagerPlugin() 
        {
            AliasOfImagaGenerator = typeof(ImageGenerator).GetField("alias", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public override void OnEnabled()
        {
            /*Harmony = new Harmony("com.cuberuben.imagegenerator");
            Harmony.PatchAll();*/

            EventHandlers = new EventHandlers(this);

            Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
        }

        public override void OnDisabled()
        {
            //Harmony.UnpatchAll();

            Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
        }
    }
}
