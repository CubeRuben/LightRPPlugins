using Exiled.API.Features;
using Exiled.API.Enums;

using System;

namespace BetterSCP096FRP
{
    public class BetterSCP096FRPPlugin : Plugin<Config>
    {
        #region Info
        public override string Name { get; } = "Better SCP-096 FRP";
        public override string Prefix { get; } = "BSCP096FRP";
        public override string Author { get; } = "CubeRuben";
        public override PluginPriority Priority { get; } = PluginPriority.Default;
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(2, 1, 0);
        #endregion

        public BetterSCP096FRPPlugin() 
        {

        }

        public override void OnEnabled()
        {
            Exiled.Events.Handlers.Scp096
        }

        public override void OnDisabled()
        {
            
        }
    }
}
