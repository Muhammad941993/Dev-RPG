using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Fader : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate()
        {
            _canvasGroup.alpha = 1;
        }
        
        public IEnumerator FadeOut(float duration)
        {
            while (_canvasGroup.alpha < 1)
            {
                _canvasGroup.alpha += Time.deltaTime / duration;
                yield return null;
            }
        }

        public IEnumerator FadeIn(float duration)
        {
            while (_canvasGroup.alpha > 0)
            {
                _canvasGroup.alpha -= Time.deltaTime / duration;
                yield return null;
            }

            _canvasGroup.alpha = 0;
        }
    }
}