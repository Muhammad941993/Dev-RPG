using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;

namespace RPG.savingSystem
{
    public class SavingSystem : MonoBehaviour
    {
        public IEnumerator LoadLastScene(string filename)
        {
            var oldState = LoadFile(filename);
            if (oldState.TryGetValue("lastSceneBuildIndex", out var value))
            {
                var sceneIndex = (int)value;
                if(sceneIndex != SceneManager.GetActiveScene().buildIndex)
                    yield return SceneManager.LoadSceneAsync(sceneIndex);
            }
            RestoreState(oldState);
        }
        public void Save(string fileName)
        {
            var oldState = LoadFile(fileName);
            CaptureState(oldState);
            Save(fileName,oldState);
        }
        
        public void Load(string fileName)
        {
            RestoreState(LoadFile(fileName));
        }

        private Dictionary<string, object> LoadFile(string fileName)
        {
            var path = GatPathFromSaveFile(fileName);
            
            if(!File.Exists(path)) return new Dictionary<string, object>();

            using var reads = File.Open(path, FileMode.Open);

            var bf = new BinaryFormatter();

            return (Dictionary<string, object>) bf.Deserialize(reads);
        }

        public void Save(string fileName, object captureState)
        {
            var path = GatPathFromSaveFile(fileName);
            using var writes = File.Open(path, FileMode.Create);
            var bf = new BinaryFormatter();
            bf.Serialize(writes, captureState);
        }


        private void CaptureState(Dictionary<string, object> oldState)
        {
            foreach (var entity in FindObjectsByType<SaveableEntity>(FindObjectsSortMode.None))
                oldState[entity.GetUniqueIdentifier()] = entity.CaptureState();

            oldState["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            var x = FindObjectsByType<SaveableEntity>(FindObjectsSortMode.None);

            foreach (var stateEntity in x)
            { 
                if(!state.ContainsKey(stateEntity.GetUniqueIdentifier())) continue;
                stateEntity.RestoreState(state[stateEntity.GetUniqueIdentifier()]);
            }
        }

        private string GatPathFromSaveFile(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName + ".sav");
        }
    }
}