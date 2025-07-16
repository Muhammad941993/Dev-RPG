using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        private Text _experienceText;
        private Experience _experience;

        private void Awake()
        {
            _experience = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
            _experienceText = GetComponent<Text>();
        }

        // Update is called once per frame
        private void Update()
        {
            _experienceText.text = $"{_experience.GetExperience():0}";
        }
    }
}