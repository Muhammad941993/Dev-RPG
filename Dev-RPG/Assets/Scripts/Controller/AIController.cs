using System;
using RPG.Attribute;
using RPG.Combat;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance;
        [SerializeField] private float timeAtPatrolPosition;
        [Range(0, 1)] [SerializeField] private float patrolSpeedFraction;
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField]private float suspiciousTime = 5;
        [SerializeField] private float aggroCollDownTime = 5;
        [SerializeField] private float shoutDistance = 5;
        private Vector3 _currentPatrolPosition;

        private int _currentPatrolPositionIndex;
        private Fighter _fighter;

        private Vector3 _guardPosition;
        private Health _health;
        private float _lastTimeScienceChase = Mathf.Infinity;
        private float _timeScienceAggravated = Mathf.Infinity;

        private Mover _mover;

        private Transform _target;
        private float _timeScienceArrivePatrolPosition = Mathf.Infinity;

        private void Awake()
        {
            _target = GameObject.FindGameObjectWithTag("Player").transform;
            _health = GetComponent<Health>();
            _mover = GetComponent<Mover>();
            _fighter = GetComponent<Fighter>();
        }

        private void Start()
        {
            _guardPosition = transform.position;
        }

        // Update is called once per frame
        private void Update()
        {
            if (_health.IsDead) return;

            if (IsTargetInRange())
                AttackBehaviour();
            else if (_lastTimeScienceChase < suspiciousTime)
                SuspiciousBehaviour();
            else
                PatrolBehaviour();

            UpdateTimers();
        }

        public void Aggravate()
        {
            _timeScienceAggravated = 0;
        }


        private void PatrolBehaviour()
        {
            _currentPatrolPosition = _guardPosition;
            if (patrolPath != null)
            {
                if (AtPatrolPosition())
                {
                    _timeScienceArrivePatrolPosition = 0;
                    _currentPatrolPositionIndex = patrolPath.GetNextIndex(_currentPatrolPositionIndex);
                }

                _currentPatrolPosition = GetCurrentPatrolPosition();
            }

            if (_timeScienceArrivePatrolPosition > timeAtPatrolPosition)
                _mover.StartMoveAction(_currentPatrolPosition, patrolSpeedFraction);
        }

        private Vector3 GetCurrentPatrolPosition()
        {
            return patrolPath.GetPatrolPosition(_currentPatrolPositionIndex);
        }

        private bool AtPatrolPosition()
        {
            return Vector3.Distance(transform.position, GetCurrentPatrolPosition()) < 1f;
        }

        private void SuspiciousBehaviour()
        {
            _mover.StartMoveAction(transform.position, patrolSpeedFraction);
        }

        private void AttackBehaviour()
        {
            _lastTimeScienceChase = 0;
            _fighter.Attack(_target.gameObject);
            AggravateNearbyEnemies();
        }

        private void AggravateNearbyEnemies()
        {
            var colliders = Physics.OverlapSphere(transform.position, shoutDistance);
            AIController aiController;
            foreach (var collider in colliders)
            {
                aiController = collider.GetComponent<AIController>();
                if(aiController == null) continue;
                aiController.Aggravate();
            }
        }

        private bool IsTargetInRange()
        {
            return Vector3.Distance(transform.position, _target.position) <= chaseDistance || _timeScienceAggravated < aggroCollDownTime;
        }

        private void UpdateTimers()
        {
            _lastTimeScienceChase += Time.deltaTime;
            _timeScienceArrivePatrolPosition += Time.deltaTime;
            _timeScienceAggravated += Time.deltaTime;
        }
    }
}