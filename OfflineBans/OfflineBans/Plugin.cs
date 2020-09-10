using Exiled.API.Features;

namespace OfflineBans
{
    public class MainSettings : Plugin<Config>
    {
        public override string Name => nameof(OfflineBans);
        public EventHandlers EventHandlers { get; set; }
        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers();
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += EventHandlers.OnSendingRemoteAdminCommand;
            Log.Info(Name + " on");
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= EventHandlers.OnSendingRemoteAdminCommand;
            Log.Info(Name + " off");
        }
    }
}