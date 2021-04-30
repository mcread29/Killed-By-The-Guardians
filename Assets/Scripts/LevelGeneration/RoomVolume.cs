using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UntitledFPS
{
    [ExecuteInEditMode]
    public class RoomVolume : MonoBehaviour
    {
        [SerializeField] private Vector3 m_roomSize = Vector3.zero;
        [SerializeField] private int m_roomScale = 1;

        [SerializeField] private Transform m_voxelParent;

#if UNITY_EDITOR
        [ContextMenu("CHECK SELECTIONS")]
        public void CheckSelections()
        {
            if (Selection.gameObjects.Length != 2)
            {
                Debug.LogWarning("MUST HAVE 2 ROOMS SELECTED");
                return;
            }
            List<RoomVolume> volumes = new List<RoomVolume>();
            foreach (GameObject obj in Selection.gameObjects)
            {
                RoomVolume vol = obj.GetComponent<RoomVolume>();
                if (vol == null)
                {
                    Debug.LogWarning("ONE OF THE SELECTIONS ISNT A ROOMVOLUME");
                    return;
                }
                volumes.Add(vol);
            }

            bool overlap = volumes[0].CheckVolume(volumes[1], true);
            Debug.Log("OVERLAP: " + overlap);
        }
#endif

        public bool CheckVolume(RoomVolume volume, bool debug = false)
        {
            Vector3 distance = volume.transform.position - transform.position;

            Vector3 ourSize = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one) * m_roomSize;
            Vector3 theirSize = Matrix4x4.TRS(volume.transform.position, volume.transform.rotation, Vector3.one) * volume.m_roomSize;

            if (
                Mathf.Abs(distance.x) > (Mathf.Abs(ourSize.x) * m_roomScale / 2f) + (Mathf.Abs(theirSize.x) * volume.m_roomScale / 2f) ||
                Mathf.Abs(distance.y) > (Mathf.Abs(ourSize.y) * m_roomScale / 2f) + (Mathf.Abs(theirSize.y) * volume.m_roomScale / 2f) ||
                Mathf.Abs(distance.z) > (Mathf.Abs(ourSize.z) * m_roomScale / 2f) + (Mathf.Abs(theirSize.z) * volume.m_roomScale / 2f)
                )
            {
                if (debug) Debug.Log("TOO FAR AWAY TO CHECK VOXELS: " + distance + ", " + ourSize + ", " + theirSize);
                return false;
            }

            for (int i = 0; i < m_voxelParent.childCount; i++)
            {
                for (int j = 0; j < volume.m_voxelParent.childCount; j++)
                {
                    VoxelVolume ours = m_voxelParent.GetChild(i).GetComponent<VoxelVolume>();
                    VoxelVolume theirs = volume.m_voxelParent.GetChild(j).GetComponent<VoxelVolume>();
                    if (ours.CheckOverlap(theirs, debug))
                    {
                        if (debug)
                        {
                            ours.name = "OVERLAP";
                            theirs.name = "OVERLAP";
                            Debug.Log("OVERLAP AT: " + transform.name + " " + i + " | " + volume.transform.name + " " + j);
                        }
                        return true;
                    }
                }
            }
            return false;
        }

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

        public void Clear()
        {
            if (m_voxelParent.childCount > 0)
            {
                GameObject.DestroyImmediate(m_voxelParent.GetChild(0).gameObject);
                if (m_voxelParent.childCount > 0) Clear();
            }
        }

        private static bool m_drawVolume = true;

        [ContextMenu("Toggle Volume")]
        public void ToggleVisual()
        {
            m_drawVolume = !m_drawVolume;
        }

        private void OnDrawGizmos()
        {
            if (m_drawVolume)
            {
#if UNITY_EDITOR
                Color gizmoColors = Gizmos.color;
                Matrix4x4 gizmoMatrix = Gizmos.matrix;

                Gizmos.color = Color.red;
                Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
                Gizmos.matrix = rotationMatrix;

                Gizmos.DrawWireCube(Vector3.zero, m_roomSize * m_roomScale);

                Gizmos.color = gizmoColors;
                Gizmos.matrix = gizmoMatrix;
#endif
            }
        }
    }
}
