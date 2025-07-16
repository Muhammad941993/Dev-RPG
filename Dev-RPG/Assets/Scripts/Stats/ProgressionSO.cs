using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/Progression")]
    public class ProgressionSO : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterData[] characterData;
        
        private Dictionary<CharacterClass, Dictionary<StatType, float[]>> _lookUpTable;
        
        public float GetStat(StatType statType,CharacterClass characterClass, int level)
        {
            BuildLookUpTable();

            var levels = _lookUpTable[characterClass][statType];

            if (levels.Length < level)
            {
                return 0;
            }
            return levels[level-1];
        }

        public int GetLevels(CharacterClass characterClass, StatType statType)
        {
            BuildLookUpTable();
            return _lookUpTable[characterClass][statType].Length;
        }

        private void BuildLookUpTable()
        {
            if(_lookUpTable != null) return;
            _lookUpTable = new Dictionary<CharacterClass, Dictionary<StatType, float[]>>();
            foreach (var character in characterData)
            {
                var newDic = new Dictionary<StatType, float[]>();
                foreach (var characterStat in character.stats)
                {
                    newDic[characterStat.statType] = characterStat.levels;
                }
                _lookUpTable[character.characterClass] = newDic;
            }
        }
    }

    [Serializable]
    public class ProgressionCharacterData
    {
        public CharacterClass characterClass;
        public ProgressionStat[] stats;
    }
    
    [Serializable]
    public class ProgressionStat
    {
        [FormerlySerializedAs("stat")] public StatType statType;
        public float[] levels;
    }
}