using System;
using GameDevTV.Utils;
using Stats;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)] [SerializeField] private int startLevel = 1;

        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private ProgressionSO progressionSo;
        [SerializeField] private GameObject levelUpEffect;
        [SerializeField] private bool shouldUseModifiers;
        
        public Action OnLevelUp;
        private LazyValue<int> _currentLevel;
        private Experience _experience;

        private void Awake()
        {
            _experience = GetComponent<Experience>();
            _currentLevel = new LazyValue<int>(CalculateLevel);
        }

        private void OnEnable()
        {
            if (_experience != null)
                _experience.OnGainExperience += UpdateLevel;
        }

        private void OnDisable()
        {
            if (_experience != null)
                _experience.OnGainExperience -= UpdateLevel;
        }

        private void Start()
        {
            _currentLevel.ForceInit();
        }

        public int GetLevel()
        {
            return _currentLevel.value;
        }

        public float GetStat(StatType statType)
        {
            var baseStat = progressionSo.GetStat(statType, characterClass, GetLevel());
            var percentageBonus = 1 + GetPercentageBonus(statType)/100;
            var additiveStat = GetAdditiveModifier(statType);
            var total = (baseStat + additiveStat) * percentageBonus;
            return total;
        }

        private float GetPercentageBonus(StatType statType)
        {
            if (!shouldUseModifiers) return 0;
            
            var totalBonus = 0f;
            foreach (var provider in GetComponents<IModifierProvider>())
            {
                foreach (var modifier in provider.GetPercentageModifiers(statType))
                {
                    totalBonus += modifier;
                }
            }
            return totalBonus;
        }

        private float GetAdditiveModifier(StatType statType)
        {
            if (!shouldUseModifiers) return 0;

            var totalAdditiveModifier = 0f;
            foreach (var provider in GetComponents<IModifierProvider>())
            {
                foreach (var modifier in provider.GetAdditiveModifiers(statType))
                {
                    totalAdditiveModifier += modifier;
                }
            }
            return totalAdditiveModifier;
        }

        private void UpdateLevel()
        {
            var newLevel = CalculateLevel();
            if (newLevel > _currentLevel.value)
            {
                _currentLevel.value = newLevel;
                OnLevelUp?.Invoke();
                EnableLevelUpEffect();
            }
        }

        private void EnableLevelUpEffect()
        {
            Instantiate(levelUpEffect, transform);
        }

        private int CalculateLevel()
        {
            if (_experience == null) return startLevel;
            
            var xp = _experience.GetExperience();

            var levels = progressionSo.GetLevels(characterClass,StatType.ExperienceToLevelUp);
            for (int i = 1; i <= levels; i++)
            {
              var xpLevelUp =  progressionSo.GetStat(StatType.ExperienceToLevelUp, characterClass, i);
              if (xpLevelUp > xp) return i;
            }
            return levels +1;
        }

    }
}