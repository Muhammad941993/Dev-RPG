using System;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
   private void OnDrawGizmos()
   {
      for (int i = 0; i < transform.childCount; i++)
      {
         Gizmos.DrawSphere(GetPatrolPosition(i), 0.2f);
         Gizmos.DrawLine(GetPatrolPosition(i), GetPatrolPosition(GetNextIndex(i)));
      }
   }

   public int GetNextIndex(int currentIndex)
   {
      return currentIndex + 1 >= transform.childCount ? 0 : currentIndex + 1;
   }
   public Vector3 GetPatrolPosition (int index) => transform.GetChild(index).position;

  
}
