using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematic
{
    public class CinematicTrigger : MonoBehaviour
    {
        [SerializeField] private PlayableDirector director;
        private bool _isTriggered;

        private void OnTriggerEnter(Collider other)
        {
            if (_isTriggered || !other.CompareTag("Player")) return;
            _isTriggered = true;
            director.Play();
        }
    }
}