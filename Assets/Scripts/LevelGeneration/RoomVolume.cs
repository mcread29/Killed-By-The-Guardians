using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class RoomVolume : MonoBehaviour
    {
        [SerializeField] private Vector3 m_roomSize = Vector3.zero;
        [SerializeField] private int m_roomScale = 1;

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            Color gizmoColors = Gizmos.color;
            Gizmos.color = Color.red;

            Vector3 size = m_roomSize;
            if (transform.rotation.eulerAngles.y / 90f == 1 || transform.rotation.eulerAngles.y / 90f == 3)
                size = new Vector3(m_roomSize.z, m_roomSize.y, m_roomSize.x);
            Gizmos.DrawWireCube(transform.position, size * m_roomScale);

            Gizmos.color = gizmoColors;
#endif
        }
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
    }
}
