using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using UnityEngine;

namespace BadgeSystem
{
	public class SetEvents
	{
		internal void OnWaitingForPlayers()
		{
			Global.surnameInGame = new List<string>();
			try
			{
				Global.fixedIdAndName = File.ReadAllLines(Path.Combine(Global.GetDataFolder(), Global.fileNameFixed), Encoding.UTF8).ToList();
				Global.randomName = File.ReadAllLines(Path.Combine(Global.GetDataFolder(), Global.fileNameRandom), Encoding.UTF8).ToList();
				Global.Active = true;
				Log.Info("BadgeSystem's data has been successfully downloaded");
			}
			catch (Exception)
			{
				Global.Active = false;
				Log.Info("Error loading names. BadgeSystem was disabled. See you next round, exile...");
			}
			try
			{
				Global.color = File.ReadAllText(Path.Combine(Global.GetDataFolder(), Global.fileNameColor), Encoding.UTF8);
				Log.Info("Successfully download custom action color: " + Global.color);
			}
			catch (Exception)
			{
				Global.color = "army_green";
				Log.Info("Failed download custom action color. Set default action color: " + Global.color);
			}
		}

		internal void OnSpawning(SpawningEventArgs ev)
		{
			if (Global.Active)
			{
				if (!ev.Player.GameObject.GetComponent<BadgeSystemComponent>()) 
				{
					ev.Player.GameObject.AddComponent<BadgeSystemComponent>();
				}

				foreach (string item in Global.fixedIdAndName)
				{
					Log.Info(ev.Player.ReferenceHub.queryProcessor.PlayerId);
					Log.Info(Player.Get(ev.Player.ReferenceHub.queryProcessor.PlayerId));
					if (item.Contains(ev.Player.UserId.Replace("@steam", string.Empty)))
					{
						if (item.Split(' ').Length != 2)
						{
							break;
						}
						ev.Player.ReferenceHub.nicknameSync.Network_displayName = ev.Player.ReferenceHub.nicknameSync.MyNick + item.Split(' ')[1];
						
						return;
					}
				}
				ev.Player.ReferenceHub.nicknameSync.Network_displayName = ev.Player.ReferenceHub.nicknameSync.MyNick + Global.SetSurName();
			}
		}

		internal void OnSendingRemoteAdminCommand(SendingRemoteAdminCommandEventArgs ev)
		{
			if (!(ev.Sender.GameObject.GetComponent<BadgeSystemComponent>() == null))
			{
				if (ev.Name == "hidetag")
				{
					ev.Sender.GameObject.GetComponent<BadgeSystemComponent>()
						.IsBadgeCover = true;
					ev.Sender.GameObject.GetComponent<BadgeSystemComponent>()
						.IsRefreshBadgeCover = true;
				}
				else if (ev.Name == "showtag")
				{
					ev.Sender.GameObject.GetComponent<BadgeSystemComponent>()
						.IsBadgeCover = false;
					ev.Sender.GameObject.GetComponent<BadgeSystemComponent>()
						.IsRefreshBadgeCover = true;
				}
			}
		}
	}
}
