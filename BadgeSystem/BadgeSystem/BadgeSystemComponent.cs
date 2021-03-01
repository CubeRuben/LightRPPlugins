using BetterTranquilizer;
using BleedOutPlugin;
using Exiled.API.Features;
using PocketKillsPlugin;
using UnityEngine;

namespace BadgeSystem
{
	public class BadgeSystemComponent : MonoBehaviour
	{
		private float Timer = 0f;

		private readonly float TimeIsUp = 0.5f;

		private int Stage = 0;

		private string Badge = null;

		private string Color = null;

		public bool IsBadgeCover = false;

		public bool IsRefreshBadgeCover = false;

		public void Update()
		{
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Invalid comparison between Unknown and I4
			Timer += Time.deltaTime;
			if (!(Timer > TimeIsUp))
			{
				return;
			}
			Timer = 0f;
			if (Badge == null && Color == null)
			{
				Badge = ((Component)this).gameObject.GetComponent<ServerRoles>().NetworkMyText;
				Color = ((Component)this).gameObject.GetComponent<ServerRoles>().NetworkMyColor;
			}
			if (Player.Get(((Component)this).gameObject) != null && (int)Player.Get(((Component)this).gameObject).Role == 2)
			{
				if (IsBadgeCover)
				{
					if (Stage != 0 || IsRefreshBadgeCover)
					{
						SetRank("white", string.Empty);
						Stage = 0;
					}
				}
				else if (Stage != 3 || IsRefreshBadgeCover)
				{
					SetRank(Color, Badge);
					Stage = 3;
				}
			}
			else if ((Object)(object)((Component)this).gameObject.GetComponent<PocketKillsComponent>() != (Object)null)
			{
				if (Stage != 1 || IsRefreshBadgeCover)
				{
					if (IsBadgeCover)
					{
						SetRank(Global.color, Global.pocketkills);
					}
					else
					{
						SetRank(Global.color, Badge + Global.voidSymbol + Global.pocketkills);
					}
					Stage = 1;
				}
			}
			else if ((Object)(object)((Component)this).gameObject.GetComponent<BadgeComponent>() != (Object)null)
			{
				if (Stage != 2 || IsRefreshBadgeCover)
				{
					if (IsBadgeCover)
					{
						SetRank(Global.color, Global.bodyholder);
					}
					else
					{
						SetRank(Global.color, Badge + Global.voidSymbol + Global.bodyholder);
					}
					Stage = 2;
				}
			}
			else if ((Object)(object)((Component)this).gameObject.GetComponent<BleedOutComponent>() != (Object)null)
			{
				if (Stage != 2 || IsRefreshBadgeCover)
				{
					if (IsBadgeCover)
					{
						SetRank(Global.color, Global.bleedout);
					}
					else
					{
						SetRank(Global.color, Badge + Global.voidSymbol + Global.bleedout);
					}
					Stage = 2;
				}
			}
			else if (IsBadgeCover)
			{
				if (Stage != 0 || IsRefreshBadgeCover)
				{
					SetRank("white", string.Empty);
					Stage = 0;
				}
			}
			else if (Stage != 3 || IsRefreshBadgeCover)
			{
				SetRank(Color, Badge);
				Stage = 3;
			}
		}

		private void SetRank(string color, string badge)
		{
			((Component)this).gameObject.GetComponent<ServerRoles>().NetworkMyColor = color;
			((Component)this).gameObject.GetComponent<ServerRoles>().NetworkMyText = badge;
		}

		public BadgeSystemComponent()
		{
		}
	}
}
