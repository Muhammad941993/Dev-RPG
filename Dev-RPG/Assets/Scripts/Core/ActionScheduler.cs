using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        private IAction _currentAction;

        public void StartAction(IAction action)
        {
            if (_currentAction == action) return;
            
            if (_currentAction != null)
            {
                _currentAction.Cancle();
            }
            _currentAction = action;
        }

        public void CancleCurrentAction()
        {
            StartAction(null);
        }
    }
}