using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private float weaponRange;
        [SerializeField] private float timeBetweenAttacks;
        [SerializeField] private float weaponDamage;
        private float _timeSinceLastAttack;
        private Health _target;
        private Mover _movement;
        private ActionScheduler _actionScheduler;
        private Animator _animator;


        private readonly int _attackHash = Animator.StringToHash("attack");
        private readonly int _stopAttackHash = Animator.StringToHash("stopAttack");

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _movement = GetComponent<Mover>();
            _actionScheduler = GetComponent<ActionScheduler>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (!CanAttack(_target)) return;

            _timeSinceLastAttack += Time.deltaTime;

            if (!IsTargetInRange())
            {
                _movement.MovementTo(_target.transform.position);
            }
            else
            {
                _movement.Cancle();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            if (_timeSinceLastAttack < timeBetweenAttacks) return;
            _timeSinceLastAttack = 0;
            transform.LookAt(_target.transform.position);
            // the animation will trigger Hit() event
            _animator.SetTrigger(_attackHash);
        }

        private bool IsTargetInRange()
        {
            return Vector3.Distance(transform.position, _target.transform.position) < weaponRange;
        }

        public void Attack(GameObject combatTarget)
        {
            var target = combatTarget?.GetComponent<Health>();
            if (!CanAttack(target)) return;
            _target = target;
            _animator.ResetTrigger(_stopAttackHash);
            _actionScheduler.StartAction(this);
        }

        public void Cancle()
        {
            _target = null;
            _animator.SetTrigger(_stopAttackHash);
        }

        // animation Hit
        private void Hit()
        {
            _target?.TakeDamage(weaponDamage);
        }

        public bool CanAttack(Health target)
        {
            if(target == null || target.IsDead) return false;
            return true;
        }
    }
}