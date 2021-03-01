using System;
using System.IO;
using System.Linq;
using System.Text;
using Exiled.API.Features;
using Exiled.Events;
using Exiled.Events.EventArgs;
using Exiled.Events.Handlers;

namespace BadgeSystem
{
	public class MainSettings : Plugin<Config>
	{
		public override string Name => "BadgeSystem";

		public SetEvents SetEvents
		{
			get;
			set;
		}

		public override void OnEnabled()
		{
			SetEvents = new SetEvents();
			try
			{
				Global.color = File.ReadAllText(Path.Combine(Global.GetDataFolder(), Global.fileNameColor), Encoding.UTF8);
				Log.Info((object)("Successfully download custom action color: " + Global.color));
			}
			catch (Exception)
			{
				Global.color = "army_green";
				Log.Info((object)("Failed download custom action color. Set default action color: " + Global.color));
			}
			try
			{
				Global.fixedIdAndName = File.ReadAllLines(Path.Combine(Global.GetDataFolder(), Global.fileNameFixed), Encoding.UTF8).ToList();
				Global.randomName = File.ReadAllLines(Path.Combine(Global.GetDataFolder(), Global.fileNameRandom), Encoding.UTF8).ToList();
				Exiled.Events.Handlers.Player.Spawning += SetEvents.OnSpawning;
				Exiled.Events.Handlers.Server.WaitingForPlayers += SetEvents.OnWaitingForPlayers;
				Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += SetEvents.OnSendingRemoteAdminCommand;
				Log.Info((object)(((Plugin<Config>)this).Name + " on"));
			}
			catch (Exception)
			{
				Log.Info((object)"Error loading names. Plugin was disabled");
			}
		}

		public override void OnDisabled()
		{
			Exiled.Events.Handlers.Player.Spawning -= SetEvents.OnSpawning;
			Exiled.Events.Handlers.Server.WaitingForPlayers -= SetEvents.OnWaitingForPlayers;
			Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= SetEvents.OnSendingRemoteAdminCommand;
			Log.Info((object)(((Plugin<Config>)this).Name + " off"));
		}
	}
}
