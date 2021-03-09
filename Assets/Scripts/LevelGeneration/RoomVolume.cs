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

        public bool CheckVolume(RoomVolume volume, bool debug = false)
        {
            Vector3 distance = volume.transform.position - transform.position;
            Vector3 ourSize = m_roomSize;
            if (transform.rotation.eulerAngles.y / 90f == 1 || transform.rotation.eulerAngles.y / 90f == 3)
                ourSize = new Vector3(m_roomSize.z, m_roomSize.y, m_roomSize.x);

            Vector3 theirSize = volume.m_roomSize;
            if (volume.transform.rotation.eulerAngles.y / 90f == 1 || volume.transform.rotation.eulerAngles.y / 90f == 3)
                theirSize = new Vector3(volume.m_roomSize.z, volume.m_roomSize.y, volume.m_roomSize.x);

            if (
                Mathf.Abs(distance.x) > (ourSize.x * m_roomScale / 2) + (theirSize.x * volume.m_roomScale / 2) ||
                Mathf.Abs(distance.y) > (ourSize.y * m_roomScale / 2) + (theirSize.y * volume.m_roomScale / 2) ||
                Mathf.Abs(distance.z) > (ourSize.z * m_roomScale / 2) + (theirSize.z * volume.m_roomScale / 2)
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
                    if (ours.CheckOverlap(theirs, debug)) return true;
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
}
