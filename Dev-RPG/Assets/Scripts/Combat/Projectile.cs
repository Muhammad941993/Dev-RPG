using RPG.Attribute;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float lifetime = 5;
        [SerializeField] private float speed;
        [SerializeField] private bool isHoming;
        [SerializeField] private GameObject impactEffect;
        [SerializeField] private UnityEvent onHit;
        private float _damage;

        private GameObject _instigator;
        private Health _target;

        private void Start()
        {
            transform.LookAt(TargetLocation());
        }

        private void Update()
        {
            transform.Translate(Vector3.forward * (speed * Time.deltaTime));

            if (isHoming && _target.IsDead)
                transform.LookAt(TargetLocation());
        }

        private void OnTriggerEnter(Collider other)
        {
            var target = other.GetComponent<Health>();
            if (target == null || target != _target || target.IsDead) return;
            onHit?.Invoke();
            _target.TakeDamage(_instigator,_damage);
            speed = 0;

            if (impactEffect != null) Instantiate(impactEffect, TargetLocation(), Quaternion.identity);
            Destroy(gameObject);
        }


        public void SetTarget(GameObject instigator,Health target, float damage)
        {
            _target = target;
            _damage = damage;
            _instigator = instigator;
            Destroy(gameObject, lifetime);
        }

        private Vector3 TargetLocation()
        {
            if (_target == null)
            {
                Destroy(gameObject);
                return Vector3.zero;
            }

            var targetCollider = _target.GetComponent<CapsuleCollider>();

            if (targetCollider == null) return _target.transform.position;
            return _target.transform.position + Vector3.up * targetCollider.height / 2;
        }
    }
}