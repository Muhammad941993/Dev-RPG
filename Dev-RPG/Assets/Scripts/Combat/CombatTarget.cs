using RPG.Attribute;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour,IRayCastable
    {
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool HandleRayCast(PlayerController player)
        {
            if (Input.GetMouseButtonDown(0))
            {
                player.GetComponent<Fighter>().Attack(gameObject);
            }
            return true;
        }
    }
}