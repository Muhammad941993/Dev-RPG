using UnityEngine;

namespace RPG.Attribute
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Health health;
        [SerializeField] private RectTransform healthBar;
        [SerializeField] private Canvas rootCanvas;
        

        private void Update()
        {
            if (Mathf.Approximately(health.GetFraction(), 0) || Mathf.Approximately(health.GetFraction(), 1))
            {
                rootCanvas.enabled = false;
                return;
            }
            rootCanvas.enabled = true;
            healthBar.localScale = new Vector3(health.GetFraction(), 1,1);

        }
    }
}