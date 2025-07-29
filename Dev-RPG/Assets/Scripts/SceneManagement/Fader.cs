using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Fader : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        private Coroutine _activeCoroutine;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate()
        {
            _canvasGroup.alpha = 1;
        }

        public Coroutine FadeOut(float duration)
        {
            return Fade(1,duration);
        }
        
        public Coroutine FadeIn(float duration)
        {
            return Fade(0,duration);
        }

        private Coroutine Fade(float target, float duration)
        {
            if (_activeCoroutine != null) StopCoroutine(_activeCoroutine);
            
            _activeCoroutine = StartCoroutine(FadeRoutine(target,duration));
            return _activeCoroutine;
        }
        
        private IEnumerator FadeRoutine(float target ,float duration)
        {
            while (!Mathf.Approximately(_canvasGroup.alpha, target))
            {
                _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, target, Time.deltaTime / duration);
                yield return null;
            }
        }
    }
}