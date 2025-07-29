using System;
using RPG.Attribute;
using RPG.Combat;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float castRadius = 1f;
        [SerializeField] private CursorMapping[] cursorMappings;

        private Fighter _fighter;
        private Health _health;
        private RaycastHit _hit;
        private Mover _mover;
        private RaycastHit[] _rayHits;
        private Camera _targetCamera;

        private void Awake()
        {
            _targetCamera = Camera.main;
            _health = GetComponent<Health>();
            _mover = GetComponent<Mover>();
            _fighter = GetComponent<Fighter>();
        }

        // Update is called once per frame
        private void Update()
        {
            if(InteractWithUI()) return;
            
            if (_health.IsDead)
            {
                SetCursor(CursorType.None);
                return;
            }
            
            if(InteractWithComponent()) return;
            if (InteractWithMovement()) return;
            
            SetCursor(CursorType.None);
        }

        private bool InteractWithComponent()
        {
            SortAllRaycastHits();
            foreach (var hit in _rayHits)
            {
                var iRays = hit.collider.GetComponents<IRayCastable>();

                foreach (var iRay in iRays)
                {
                    if (!iRay.HandleRayCast(this)) continue;
                    SetCursor(iRay.GetCursorType());
                    return true;
                }
            }
            return false;
        }

        private void SortAllRaycastHits()
        {
            _rayHits = Physics.SphereCastAll(GetMouseRay() , castRadius);
            var distances = new float[_rayHits.Length];
            for (int i = 0; i < _rayHits.Length; i++)
            {
                distances[i] = _rayHits[i].distance;
            }
            Array.Sort(distances,_rayHits);
        }

        private bool InteractWithUI()
        {
            if (!EventSystem.current.IsPointerOverGameObject()) return false;
            SetCursor(CursorType.UI);
            return true;
        }

        private bool InteractWithMovement()
        {
            if (RaycastToNavmesh(out var position))
            {
                if(!_mover.CanMoveTo(position)) return false;
                
                if (Input.GetMouseButton(0))
                    _mover.StartMoveAction(position, 1);

                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private bool RaycastToNavmesh(out Vector3 position)
        {
            position = new Vector3();

            if (!Physics.Raycast(GetMouseRay(), out _hit)) return false;

            var hasCastToNave = NavMesh.SamplePosition(_hit.point, out var navHit, 1f, NavMesh.AllAreas);
            if(!hasCastToNave) return false;
            position = navHit.position;

            if (!_mover.CanMoveTo(position)) return false;
            
            return true;
        }



        private void SetCursor(CursorType cursorType)
        {
            var mapping = GetCursorMapping(cursorType);
            Cursor.SetCursor(mapping.texture,mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (var mapping in cursorMappings)
            {
                if(mapping.type == type)
                    return mapping;
            }
            return cursorMappings[0];
        }
    

    [System.Serializable]
    public struct CursorMapping
    { 
        public CursorType type;
        public Texture2D texture;
        public Vector2 hotspot;

    }
  
        private Ray GetMouseRay()
        {
            return _targetCamera.ScreenPointToRay(Input.mousePosition);
        }
    }
    public enum CursorType
    {
        None,
        Movement,
        Combat,
        UI,
        PickUp
    } 

}