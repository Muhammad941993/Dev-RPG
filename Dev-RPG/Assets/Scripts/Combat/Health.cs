using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float health = 100f;

        private Animator _animator;
        private ActionScheduler _actionScheduler;
        private readonly int _deathHash = Animator.StringToHash("die");

        public bool IsDead {get; private set;}
        
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
        }

        public void TakeDamage(float damage)
        {
            if (IsDead) return;
            
            health = Mathf.Max(health - damage, 0);
            if (health <= 0)
            {
                IsDead = true;
                _animator.SetTrigger(_deathHash);
                _actionScheduler.CancleCurrentAction();
            }
        }
    }
}