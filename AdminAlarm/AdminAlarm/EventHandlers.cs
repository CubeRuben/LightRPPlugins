using Exiled.Events.EventArgs;

namespace AdminAlarm
{
    class EventHandlers
    {
        AdminAlarmPlugin Plugin;

        public EventHandlers(AdminAlarmPlugin plugin) 
        {
            Plugin = plugin;
        }

        public void OnSendingRemoteAdminCommand(SendingRemoteAdminCommandEventArgs ev) 
        {
            switch (ev.Name) 
            {
                case "alarm":
                    ev.Sender.ShowHint("You are debil", 10);
                    break;
            }
        }
    }
}
