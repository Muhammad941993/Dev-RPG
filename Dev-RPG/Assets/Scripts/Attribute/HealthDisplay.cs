using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attribute
{
    public class HealthDisplay : MonoBehaviour
    {
        private Text _healthText;
        private Health _health;
        private float _maxHealth;

        private void Awake()
        {
            _health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
            _healthText = GetComponent<Text>();
        }

        void Start()
        {
            _maxHealth = _health.GetMaxHealth();
        }

        // Update is called once per frame
        private void Update()
        {
            _healthText.text = $"{_health.GetHealth():0} / {_maxHealth:0}";
        }
    }
}