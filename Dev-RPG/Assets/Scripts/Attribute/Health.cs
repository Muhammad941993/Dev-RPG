using System;
using GameDevTV.Utils;
using RPG.Core;
using RPG.savingSystem;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attribute
{
    public class Health : MonoBehaviour, Isaveable
    {
        [SerializeField] private float regenerationPercentage;
        [SerializeField] private UnityEvent<float> onTakeDamage;
        [SerializeField] private UnityEvent onDeath;
        private LazyValue<float> _health;
        private readonly int _deathHash = Animator.StringToHash("die");
        private ActionScheduler _actionScheduler;

        private Animator _animator;
        private BaseStats _baseStats;

        public bool IsDead { get; private set; }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _baseStats = GetComponent<BaseStats>();

            _health = new LazyValue<float>(InitHealth);

        }

        
        private void Start()
        {
            _health.ForceInit();
        }
        
        private void OnEnable()
        {
            _baseStats.OnLevelUp += OnLevelUp;
        }

        private void OnDisable()
        {
            _baseStats.OnLevelUp -= OnLevelUp;
        }

        private void OnLevelUp()
        {
            var health = _baseStats.GetStat(StatType.Health) * regenerationPercentage / 100f;
            _health.value = Mathf.Max(health, _health.value);
        }


        private float InitHealth()
        {
            return _baseStats.GetStat(StatType.Health);
        }

        public object CaptureState()
        {
            return _health.value;
        }

        public void RestoreState(object state)
        {
            _health.value = (float)state;
            Death();
        }

        public void TakeDamage(GameObject instigator,float damage)
        {
            if (IsDead) return;

            _health.value = Mathf.Max(_health.value - damage, 0);

            onTakeDamage?.Invoke(damage);

            if (_health.value > 0) return;
            
            onDeath?.Invoke();
            Death();
            AddExperienceReward(instigator);
           
        }
        public float GetHealth() => _health.value;
        public float GetMaxHealth() => _baseStats.GetStat(StatType.Health);


        public float GetHealthPercentage()
        {
            return 100 * GetFraction();
        }

        public float GetFraction() => _health.value / _baseStats.GetStat(StatType.Health);

        private void Death()
        {
            if (_health.value > 0) return;
            IsDead = true;
            _animator.SetTrigger(_deathHash);
            _actionScheduler.CancleCurrentAction();
        }
        
        private void AddExperienceReward(GameObject instigator)
        {
            var xp = _baseStats.GetStat(StatType.ExperienceReward);
            instigator?.GetComponent<Experience>()?.AddExperience(xp);
        }

        public void Heal(float healthToRestore)
        {
            _health.value = Mathf.Min(_health.value + healthToRestore, GetMaxHealth());
            
        }
    }
}