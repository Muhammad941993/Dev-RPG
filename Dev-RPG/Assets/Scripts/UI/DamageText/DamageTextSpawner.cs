using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] private DamageText damageText;
        private DamageText _damageText;

        public void SpawnDamageText(float damage)
        {
            if (_damageText == null)
                _damageText = Instantiate(damageText, transform);

            _damageText.ShowDamageText(damage);

        }

    }
}