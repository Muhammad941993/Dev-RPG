using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Camera _targetCamera;

        private Health _health;
        private Mover _mover;
        private Fighter _fighter;
        private RaycastHit[] _rayHits;
        private RaycastHit _hit;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            _targetCamera = Camera.main;
            _health = GetComponent<Health>();
            _mover = GetComponent<Mover>();
            _fighter = GetComponent<Fighter>();
        }

        // Update is called once per frame
        private void Update()
        {
            if(_health.IsDead) return;
            
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }

        private bool InteractWithCombat()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _rayHits = Physics.RaycastAll(GetMouseRay());
                foreach (var hit in _rayHits)
                {
                    var target = hit.collider.GetComponent<CombatTarget>();
                    if (target == null) continue;
                    _fighter.Attack(target.gameObject);
                    return true;
                }
            }

            return false;
        }

        private bool InteractWithMovement()
        {
            if (Physics.Raycast(GetMouseRay(), out _hit))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _mover.StartMoveAction(_hit.point);
                }

                return true;
            }

            return false;
        }


        private Ray GetMouseRay()
        {
            return _targetCamera.ScreenPointToRay(Input.mousePosition);
        }
    }
}