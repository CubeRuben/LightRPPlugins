using Exiled.Events.EventArgs;

namespace VehicleCall
{
    class EventHandlers
    {
        VehicleCallPlugin Plugin;

        public EventHandlers(VehicleCallPlugin plugin) 
        {
            Plugin = plugin;
        }

        public void OnSendingRemoteAdminCommand(SendingRemoteAdminCommandEventArgs ev) 
        {
            switch (ev.Name) 
            {
                case "heli":
                    ev.IsAllowed = false;
                    Plugin.CallHelicopter();
                    ev.ReplyMessage = "Work";
                    break;
                case "van":
                    ev.IsAllowed = false;
                    Plugin.CallCar();
                    ev.ReplyMessage = "Work";
                    break;
            }
        }


    }
}
