using Exiled.API.Features;
using Exiled.Events;
using Exiled.Events.EventArgs;
using Exiled.Events.Handlers;

namespace OfflineBans
{
	public class MainSettings : Plugin<Config>
	{
		public override string Name => "OfflineBans";

		public SetEvents SetEvents
		{
			get;
			set;
		}

		public override void OnEnabled()
		{
			SetEvents = new SetEvents();
			Server.add_SendingRemoteAdminCommand((CustomEventHandler<SendingRemoteAdminCommandEventArgs>)SetEvents.OnSendingRemoteAdminCommand);
			Log.Info((object)(((Plugin<Config>)this).get_Name() + " on"));
		}

		public override void OnDisabled()
		{
			Server.remove_SendingRemoteAdminCommand((CustomEventHandler<SendingRemoteAdminCommandEventArgs>)SetEvents.OnSendingRemoteAdminCommand);
			Log.Info((object)(((Plugin<Config>)this).get_Name() + " off"));
		}
	}
}
