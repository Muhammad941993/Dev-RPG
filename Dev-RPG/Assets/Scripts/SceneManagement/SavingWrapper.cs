using System.Collections;
using RPG.savingSystem;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string SaveFile = "Saving";
        [SerializeField] private float fadeInTime;

        private SavingSystem _savingSystem;

        public void Awake()
        {
            _savingSystem = GetComponent<SavingSystem>();
            StartCoroutine(LoadLastScene());
        }

        private IEnumerator LoadLastScene()
        {
            yield return _savingSystem.LoadLastScene(SaveFile);

            var fader = FindFirstObjectByType<Fader>();
            fader.FadeOutImmediate();
            yield return fader.FadeIn(fadeInTime);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S)) Save();

            if (Input.GetKeyDown(KeyCode.L)) Load();

            if (Input.GetKeyDown(KeyCode.Delete)) Delete();
        }

        private void Delete()
        {
            _savingSystem.Delete(SaveFile);
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