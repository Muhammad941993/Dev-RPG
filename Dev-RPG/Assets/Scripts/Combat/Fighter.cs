using RPG.Core;
using RPG.Movement;
using RPG.savingSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction , Isaveable
    {
        [SerializeField] private float timeBetweenAttacks;
        [SerializeField] private Transform rightHandPosition;
        [SerializeField] private Transform leftHandPosition;
        [SerializeField] private WeaponSO defaultWeaponSo;
        
        private float _timeSinceLastAttack;
        private Health _target;
        private Mover _movement;
        private ActionScheduler _actionScheduler;
        private Animator _animator;
        private WeaponSO _currentWeaponSo;

        private readonly int _attackHash = Animator.StringToHash("attack");
        private readonly int _stopAttackHash = Animator.StringToHash("stopAttack");

        private GameObject _oldWeapon;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _movement = GetComponent<Mover>();
            _actionScheduler = GetComponent<ActionScheduler>();

            if (!_currentWeaponSo)
            {
                EquipWeapon(defaultWeaponSo);
            }
        }

        public void EquipWeapon(WeaponSO weapon)
        {
            _currentWeaponSo = weapon;
            if (_oldWeapon != null)
            {
                Destroy(_oldWeapon);
            }
           
            _oldWeapon = _currentWeaponSo.Spawn(rightHandPosition,leftHandPosition,_animator);
        }

        // Update is called once per frame
        private void Update()
        {
            if (!CanAttack(_target)) return;

            _timeSinceLastAttack += Time.deltaTime;

            if (!IsTargetInRange())
            {
                _movement.MovementTo(_target.transform.position ,1);
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
            return Vector3.Distance(transform.position, _target.transform.position) < _currentWeaponSo.Range;
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
            _movement.Cancle();
        }

        // animation Hit
        private void Hit()
        {
            if (_currentWeaponSo.HasProjectile())
            {
                _currentWeaponSo.LaunchProjectile(rightHandPosition,leftHandPosition,_target);
            }
            else
            {
                _target?.TakeDamage(_currentWeaponSo.Damage);
            }
        }

        // animation Hit
        private void Shoot() => Hit();
        
        public bool CanAttack(Health target)
        {
            if(target == null || target.IsDead) return false;
            return true;
        }


        public object CaptureState()
        {
            return _currentWeaponSo.name;
        }

        public void RestoreState(object state)
        {
            var weaponName = state as string;
            var weaponSo = Resources.Load<WeaponSO>(weaponName);
            EquipWeapon(weaponSo);
        }
    }
}