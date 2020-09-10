using Exiled.API.Features;

namespace OfflineBans
{
    public class MainSettings : Plugin<Config>
    {
        public override string Name => nameof(OfflineBans);
        public SetEvents SetEvents { get; set; }
        public override void OnEnabled()
        {
            SetEvents = new SetEvents();
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += SetEvents.OnSendingRemoteAdminCommand;
            Log.Info(Name + " on");
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= SetEvents.OnSendingRemoteAdminCommand;
            Log.Info(Name + " off");
        }
    }
}