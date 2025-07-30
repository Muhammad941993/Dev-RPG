using System;
using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.Attribute;
using RPG.Core;
using RPG.Movement;
using RPG.savingSystem;
using RPG.Stats;
using Stats;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, Isaveable ,IModifierProvider
    {
        [SerializeField] private float timeBetweenAttacks;
        [SerializeField] private Transform rightHandPosition;
        [SerializeField] private Transform leftHandPosition;
        [SerializeField] private WeaponSO defaultWeaponSo;

        private readonly int _attackHash = Animator.StringToHash("attack");
        private readonly int _stopAttackHash = Animator.StringToHash("stopAttack");
        private ActionScheduler _actionScheduler;
        private Animator _animator;

        private WeaponSO _currentWeaponSo;
        private Mover _movement;

        private Weapon _currentWeapon;
        private Health _target;

        private float _timeSinceLastAttack;
        private BaseStats _baseStats;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _movement = GetComponent<Mover>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _baseStats = GetComponent<BaseStats>();

            _currentWeaponSo = defaultWeaponSo;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            AttachWeapon(_currentWeaponSo);
        }

        // Update is called once per frame
        private void Update()
        {
            if (!CanAttack(_target)) return;

            _timeSinceLastAttack += Time.deltaTime;

            if (!IsTargetInRange(_target.transform))
            {
                _movement.MovementTo(_target.transform.position, 1);
            }
            else
            {
                _movement.Cancel();
                AttackBehaviour();
            }
        }

        public void Cancel()
        {
            _target = null;
            _animator.SetTrigger(_stopAttackHash);
            _movement.Cancel();
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

        public void EquipWeapon(WeaponSO weapon)
        {
            _currentWeaponSo = weapon;
            AttachWeapon(weapon);
        }

        private void AttachWeapon(WeaponSO weaponSo)
        {
            if (_currentWeapon != null) Destroy(_currentWeapon.gameObject);

            _currentWeapon = weaponSo.Spawn(rightHandPosition, leftHandPosition, _animator);
        }


        private void AttackBehaviour()
        {
            if (_timeSinceLastAttack < timeBetweenAttacks) return;
            _timeSinceLastAttack = 0;
            transform.LookAt(_target.transform.position);

            // the animation will trigger Hit() event
            _animator.SetTrigger(_attackHash);
        }

        private bool IsTargetInRange(Transform target)
        {
            return Vector3.Distance(transform.position, target.position) < _currentWeaponSo.Range;
        }

        public void Attack(GameObject combatTarget)
        {
            var target = combatTarget?.GetComponent<Health>();
            if (!CanAttack(target)) return;
            _target = target;
            _animator.ResetTrigger(_stopAttackHash);
            _actionScheduler.StartAction(this);
        }
        
        public Health GetTarget() => _target;
        

        // animation Hit
        private void Hit()
        {
            var damage = _baseStats.GetStat(StatType.Damage);
            
            _currentWeapon?.OnHit();
            
            if (_currentWeaponSo.HasProjectile())
                _currentWeaponSo.LaunchProjectile(gameObject,rightHandPosition, leftHandPosition, _target , damage);
            else
                _target?.TakeDamage(gameObject,damage);
        }

        // animation Hit
        private void Shoot()
        {
            Hit();
        }

        private bool CanAttack(Health target)
        {
            if (target == null || target.IsDead) return false;
            if(!_movement.CanMoveTo(target.transform.position) && !IsTargetInRange(target.transform)) return false;
            return true;
        }

        public IEnumerable<float> GetAdditiveModifiers(StatType statType)
        {
            if (statType == StatType.Damage)
            {
                yield return _currentWeaponSo.Damage;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(StatType statType)
        {
            if (statType == StatType.Damage)
            {
                yield return _currentWeaponSo.PercentageBonus;
            }
        }
    }
}