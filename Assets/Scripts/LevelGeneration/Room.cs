using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private Door[] m_doors;
        public Door[] doors { get { return m_doors; } }

        void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            // Gizmos.color = Color.red;

            // //Draw the suspension
            // Gizmos.DrawLine(
            //     Vector3.zero,
            //     Vector3.up
            // );

            // //draw force application point
            // Gizmos.DrawWireSphere(Vector3.zero, 5f);

            // Gizmos.color = Color.white;
#endif
        }

        public Door ChooseDoor()
        {
            Door selectedDoor = null;
            List<Door> unTried = new List<Door>(m_doors);
            System.Random r = new System.Random();
            while (unTried.Count > 0 && selectedDoor == null)
            {
                int ind = r.Next(unTried.Count);
                Door d = unTried[ind];
                unTried.RemoveAt(ind);
                if (d.attached == false) selectedDoor = d;
            }
            return selectedDoor;
        }
    }
}
