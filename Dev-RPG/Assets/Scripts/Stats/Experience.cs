using System;
using RPG.savingSystem;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour , Isaveable
    {
       [SerializeField] private float experience;
       
       public Action OnGainExperience;

       public void AddExperience(float value)
       {
           experience += value;
           OnGainExperience?.Invoke();
       }

       public object CaptureState()
       {
           return experience;
       }

       public void RestoreState(object state)
       {
           experience = (float)state;
       }

       public float GetExperience()
       {
           return experience;
       }
    }
}