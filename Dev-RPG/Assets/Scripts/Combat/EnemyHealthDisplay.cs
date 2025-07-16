using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        private Text _healthText;
        private Fighter _fighter;

        private void Awake()
        {
            _fighter = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();
            _healthText = GetComponent<Text>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (_fighter.GetTarget() == null)
            {
                _healthText.text = "N/A";
                return;
            }
            
            _healthText.text = $"{_fighter.GetTarget().GetHealthPercentage():0} / {_fighter.GetTarget().GetMaxHealth()}";
        }
    }
}