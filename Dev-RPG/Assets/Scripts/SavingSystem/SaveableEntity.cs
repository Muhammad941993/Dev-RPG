using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.savingSystem
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        private static readonly Dictionary<string, SaveableEntity> GlobalLookUp = new();

        [SerializeField] private string uniqueIdentifier;

#if UNITY_EDITOR
        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;

            var serializedObject = new SerializedObject(this);
            var serializedProperty = serializedObject.FindProperty("uniqueIdentifier");

            if (string.IsNullOrEmpty(serializedProperty.stringValue) || !IsUnique(serializedProperty.stringValue))
            {
                serializedProperty.stringValue = Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            GlobalLookUp[serializedProperty.stringValue] = this;
        }
#endif
        public object CaptureState()
        {
            Dictionary<string, object> state = new();
            foreach (var save in GetComponents<Isaveable>()) state[save.GetType().ToString()] = save.CaptureState();
            return state;
        }

        public void RestoreState(object state)
        {
            var restoreState = state as Dictionary<string, object>;

            foreach (var save in GetComponents<Isaveable>())
            {
                if (!restoreState.ContainsKey(save.GetType().ToString())) continue;

                save.RestoreState(restoreState[save.GetType().ToString()]);
            }
        }

        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        private bool IsUnique(string id)
        {
            if (!GlobalLookUp.TryGetValue(id, out var value)) return true;

            if (value == this) return true;

            if (value == null)
            {
                GlobalLookUp.Remove(id);
                return true;
            }

            if (value.GetUniqueIdentifier() != id)
            {
                GlobalLookUp.Remove(id);
                return true;
            }

            return false;
        }
    }
}