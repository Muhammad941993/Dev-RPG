using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private Text _text;
        [SerializeField] private Animation _animation;

        public void ShowDamageText(float damage)
        {
            _text.text = $"{damage:0}";
            _animation.Play();
        }
    }
}