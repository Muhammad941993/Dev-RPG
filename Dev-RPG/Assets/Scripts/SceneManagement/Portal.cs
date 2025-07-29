using System.Collections;
using RPG.Control;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private int sceneToLoad = -1;
        [SerializeField] private Transform spawnPosition;
        [SerializeField] private DestinationIdentifier destination;
        [SerializeField] private float fadeOutDuration;
        [SerializeField] private float fadeInDuration;
        [SerializeField] private float fadeDuration;

        private WaitForSeconds _fadeDuration;

        private void Start()
        {
            _fadeDuration = new WaitForSeconds(fadeDuration);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            StartCoroutine(TransitionToScene());
        }

        private IEnumerator TransitionToScene()
        {
            DontDestroyOnLoad(gameObject);
            var fader = FindFirstObjectByType<Fader>();
            var saving = FindFirstObjectByType<SavingWrapper>();
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().enabled = false;

            yield return fader.FadeOut(fadeOutDuration);
            saving.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            var newPlayer = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            newPlayer.enabled = false;
            
            saving.Load();

            var portal = GetOtherPortal();
            UpdatePlayerPosition(portal);

            saving.Save();

            yield return _fadeDuration;
            fader.FadeIn(fadeInDuration);
            
            newPlayer.enabled = true;

            Destroy(gameObject);
        }

        private void UpdatePlayerPosition(Portal portal)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(portal.spawnPosition.position);
            player.transform.rotation = portal.spawnPosition.rotation;
        }

        private Portal GetOtherPortal()
        {
            foreach (var portal in FindObjectsByType<Portal>(FindObjectsSortMode.None))
            {
                if (portal == this) continue;
                if (portal.destination != destination) continue;
                return portal;
            }

            return null;
        }

        private enum DestinationIdentifier
        {
            A,
            B,
            C,
            D,
            E,
            F
        }
    }
}