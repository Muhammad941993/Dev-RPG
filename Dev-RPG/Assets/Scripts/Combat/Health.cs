using RPG.savingSystem;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour , Isaveable
    {
        [SerializeField] private float health = 100f;

        private Animator _animator;
        private ActionScheduler _actionScheduler;
        private readonly int _deathHash = Animator.StringToHash("die");

        public bool IsDead {get; private set;}
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
        }

        public void TakeDamage(float damage)
        {
            if (IsDead) return;
            
            health = Mathf.Max(health - damage, 0);
            CheckDead();
        }

        private void CheckDead()
        {
            if (health > 0) return;
            IsDead = true;
            _animator.SetTrigger(_deathHash);
            _actionScheduler.CancleCurrentAction();
        }

        public object CaptureState()
        {
            return health;
        }

        public void RestoreState(object state)
        {
            health = (float) state;
            CheckDead();
        }
    }
}