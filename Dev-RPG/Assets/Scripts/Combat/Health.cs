using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float health = 100f;

        private Animator _animator;
        private readonly int _deathHash = Animator.StringToHash("die");

        public bool IsDead {get; private set;}
        
        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void TakeDamage(float damage)
        {
            if (IsDead) return;
            
            health = Mathf.Max(health - damage, 0);
            print(health);
            if (health <= 0)
            {
                IsDead = true;
                _animator.SetTrigger(_deathHash);
            }
        }
    }
}