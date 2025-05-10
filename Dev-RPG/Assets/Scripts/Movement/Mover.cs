using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour ,IAction
    {
        private Vector3 _target;
        private NavMeshAgent _agent;
        private ActionScheduler _actionScheduler;

        [SerializeField] Animator animator;

        void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _actionScheduler = GetComponent<ActionScheduler>();
        }

        void Update()
        {
            UpdateAnimation();
        }

        private void UpdateAnimation()
        {
            Vector3 velocity = _agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            animator.SetFloat("speed", localVelocity.z);
        }

        public void MovementTo(Vector3 destination)
        {
            _agent.destination = destination;
            _agent.isStopped = false;
        }
        
        public void StartMoveAction(Vector3 hitPoint)
        {
            MovementTo(hitPoint);
            _actionScheduler.StartAction(this);

        }

        public void Cancle()
        {
            _agent.isStopped = true;
        }
    }
}