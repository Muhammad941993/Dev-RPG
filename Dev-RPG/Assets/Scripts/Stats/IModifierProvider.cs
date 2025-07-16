using System.Collections.Generic;

namespace Stats
{
    public interface IModifierProvider
    {
        public IEnumerable<float> GetAdditiveModifiers(RPG.Stats.StatType statType);
        public IEnumerable<float> GetPercentageModifiers(RPG.Stats.StatType statType);

    }
}
