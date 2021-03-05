using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class RoomVolume : MonoBehaviour
    {
        [SerializeField] private Vector3 m_roomSize = Vector3.zero;
        [SerializeField] private int m_roomScale = 1;

        [SerializeField] private Transform m_voxelParent;

        [ContextMenu("Populate")]
        public void PopulateVoxels()
        {
            if (m_voxelParent == null)
            {
                Debug.LogWarning("Please Assign Voxel Parent In Inspector");
                return;
            }

            for (int x = 0; x < m_roomSize.x; x++)
            {
                for (int y = 0; y < m_roomSize.y; y++)
                {
                    for (int z = 0; z < m_roomSize.z; z++)
                    {
                        VoxelVolume v = VoxelVolume.CreateVoxel(m_roomScale);
                        v.transform.position = new Vector3(
                            (x * m_roomScale + m_roomScale / 2f) - (m_roomSize.x * m_roomScale) / 2,
                            (y * m_roomScale + m_roomScale / 2f) - (m_roomSize.y * m_roomScale) / 2,
                            (z * m_roomScale + m_roomScale / 2f) - (m_roomSize.z * m_roomScale) / 2
                        );
                        v.transform.parent = m_voxelParent;
                    }
                }
            }
        }

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
    }
}
