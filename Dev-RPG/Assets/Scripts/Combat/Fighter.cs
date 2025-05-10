using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour ,IAction
    {
        [SerializeField] float weaponRange;
        private Transform _target;
        private Mover _movement;
        private ActionScheduler _actionScheduler;


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _movement = GetComponent<Mover>();
            _actionScheduler = GetComponent<ActionScheduler>();
        }

        // Update is called once per frame
        void Update()
        {
            if(_target == null) return;
            
            if (!IsTargetInRange())
            {
                _movement.MovementTo(_target.position);
            }
            else
            {
                _movement.Cancle();
            }
        }

        private bool IsTargetInRange()
        {
            return Vector3.Distance(transform.position, _target.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            _target = combatTarget.transform;
            _actionScheduler.StartAction(this);
        }

        public void Cancle()
        {
            _target = null;
        }
    }
}