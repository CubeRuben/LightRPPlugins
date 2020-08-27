using Exiled.Events.EventArgs;

using UnityEngine;

using Mirror;

using Dissonance.Integrations.MirrorIgnorance;
using Mirror;

namespace SoundSystem
{
    class EventHandlers
    {
        SoundSystemPlugin Plugin;

        public EventHandlers(SoundSystemPlugin plugin) 
        {
            Plugin = plugin;
        }

        public void OnSendingConsoleCommand(SendingConsoleCommandEventArgs ev) 
        {
            switch (ev.Name) 
            {
                case "test":
                    MirrorIgnoranceServer.ForceDisconnectClient(NetworkConnection.);
                    break;
                case "destroy":

                    break;
            }
        }
    }
}
