using Exiled.API.Features;
using Exiled.Events.EventArgs;
using UnityEngine;


namespace CustomEscape
{
    class EventHandlers
    {
        CustomEscapePlugin Plugin;

        public EventHandlers(CustomEscapePlugin plugin)
        { 
            Plugin = plugin;
        }

        public void OnWaitingForPlayers() 
        {
            GameObject escape = new GameObject("CustomEscapeTrigger");
            BoxCollider collider = escape.AddComponent<BoxCollider>();
            escape.AddComponent<SEscapeAnyClass>();

            collider.transform.localScale = new Vector3(4.5f, 6, 4.5f);
            collider.isTrigger = true;

            escape.layer = 8;
            escape.transform.position = new Vector3(169.8f, 986, 23.1f);

            Escape.radius = 0;
        }

        public void OnEscaping(EscapingEventArgs ev) 
        {
            ev.IsAllowed = false;
        }
    }
}
