using System.Collections;
using RPG.savingSystem;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] private float fadeInTime;
        private const string SaveFile = "Saving";

        private SavingSystem _savingSystem;

        public void Awake()
        {
            _savingSystem = GetComponent<SavingSystem>();
        }

        private IEnumerator Start()
        {
            var fader = FindFirstObjectByType<Fader>();
            fader.FadeOutImmediate();
            yield return _savingSystem.LoadLastScene(SaveFile);
            yield return fader.FadeIn(fadeInTime);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S)) Save();

            if (Input.GetKeyDown(KeyCode.L)) Load();
        }

        public void Load()
        {
            _savingSystem.Load(SaveFile);
        }

        public void Save()
        {
            _savingSystem.Save(SaveFile);
        }
    }
}