using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour ,IAction
    {
        [SerializeField] private float maxSpeed = 6;
        private Vector3 _target;
        private NavMeshAgent _agent;
        private ActionScheduler _actionScheduler;

        private Animator _animator;
        private readonly int _forwardSpeedHash = Animator.StringToHash("forwardSpeed");


        void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _animator = GetComponent<Animator>();
        }

        void Update()
        {
            UpdateAnimation();
        }

        private void UpdateAnimation()
        {
            Vector3 velocity = _agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            _animator.SetFloat(_forwardSpeedHash, localVelocity.z);
        }

        public void MovementTo(Vector3 destination , float speedFraction)
        {
            _agent.destination = destination;
            _agent.speed = maxSpeed * speedFraction;
            _agent.isStopped = false;
        }
        
        public void StartMoveAction(Vector3 hitPoint , float speedFraction)
        {
            MovementTo(hitPoint,speedFraction);
            _actionScheduler.StartAction(this);

        }

        public void Cancle()
        {
            _agent.isStopped = true;
        }
    }
}