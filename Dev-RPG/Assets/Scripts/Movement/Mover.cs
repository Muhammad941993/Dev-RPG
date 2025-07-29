using RPG.Core;
using RPG.savingSystem;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, Isaveable
    {
        [SerializeField] private float maxPathLength = 50;
        [SerializeField] private float maxSpeed = 6;
        
        
        private readonly int _forwardSpeedHash = Animator.StringToHash("forwardSpeed");
        private ActionScheduler _actionScheduler;
        private NavMeshAgent _agent;

        private Animator _animator;
        private Vector3 _target;


        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            UpdateAnimation();
        }

        public bool CanMoveTo(Vector3 targetPosition)
        {
            
            var navMeshPath = new NavMeshPath();
            var hasPath = NavMesh.CalculatePath(transform.position,targetPosition,NavMesh.AllAreas,navMeshPath);
            if (!hasPath) return false;
            if(navMeshPath.status != NavMeshPathStatus.PathComplete) return false;
            
            if(CalculatePathLength(navMeshPath) > maxPathLength) return false;

            return true;
        }
        
        private float CalculatePathLength(NavMeshPath navMeshPath)
        {
            float distance = 0;
            if(navMeshPath.corners.Length < 2) return distance;

            for (var i = 0; i < navMeshPath.corners.Length-1; i++)
            {
                distance += Vector3.Distance(navMeshPath.corners[i], navMeshPath.corners[i+1]);
            }
            return distance;
        }

        public void Cancel()
        {
            _agent.isStopped = true;
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            var serializableState = (SerializableVector3)state;
            _agent.enabled = false;
            transform.position = serializableState.ToVector3();
            _agent.enabled = true;
            _actionScheduler.CancleCurrentAction();
        }

        private void UpdateAnimation()
        {
            var velocity = _agent.velocity;
            var localVelocity = transform.InverseTransformDirection(velocity);
            _animator.SetFloat(_forwardSpeedHash, localVelocity.z);
        }

        public void MovementTo(Vector3 destination, float speedFraction)
        {
            _agent.destination = destination;
            _agent.speed = maxSpeed * speedFraction;
            _agent.isStopped = false;
        }

        public void StartMoveAction(Vector3 hitPoint, float speedFraction)
        {
            MovementTo(hitPoint, speedFraction);
            _actionScheduler.StartAction(this);
        }

    }
}