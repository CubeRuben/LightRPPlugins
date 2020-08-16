using UnityEngine;

namespace CustomEscape
{
    class SEscapeAnyClass : MonoBehaviour
    {
        void OnTriggerEnter(Collider collider) 
        {
            CharacterClassManager characterClassManager = collider.gameObject.GetComponent<CharacterClassManager>();
            if (characterClassManager)
            {
                switch (characterClassManager.CurClass)
                {
                    case RoleType.ClassD:
                        RoundSummary.escaped_ds++;
                        break;
                    case RoleType.Scientist:
                        RoundSummary.escaped_scientists++;
                        break;
                    case RoleType.NtfCadet:
                    case RoleType.NtfCommander:
                    case RoleType.NtfLieutenant:
                    case RoleType.NtfScientist:
                    case RoleType.FacilityGuard:
                        return;
                        break;
                }

                collider.gameObject.GetComponent<Inventory>().Clear();

                characterClassManager.SetPlayersClass(RoleType.Spectator, collider.gameObject);
            }
        }
    }
}
