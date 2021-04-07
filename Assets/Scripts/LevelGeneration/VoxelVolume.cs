using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UntitledFPS
{
    public class VoxelVolume : MonoBehaviour
    {
        [HideInInspector]
        [SerializeField] private float m_size = 0;
        public float size { get { return m_size; } }

        private static bool m_drawVolume = true;

        float xMin { get { return transform.position.x - (m_size / 2); } }
        float xMax { get { return transform.position.x + (m_size / 2); } }

        float yMin { get { return transform.position.y - (m_size / 2); } }
        float yMax { get { return transform.position.y + (m_size / 2); } }

        float zMin { get { return transform.position.z - (m_size / 2); } }
        float zMax { get { return transform.position.z + (m_size / 2); } }

        public static VoxelVolume CreateVoxel(float size)
        {
            GameObject obj = new GameObject("VoxelVolume");
            VoxelVolume voxel = obj.AddComponent<VoxelVolume>();
            voxel.m_size = size;
            return voxel;
        }

        [ContextMenu("CHECK SELECTIONS")]
        public void CheckSelections()
        {
            Debug.Log("WHAT IN TARNATION");
            if (Selection.gameObjects.Length != 2)
            {
                Debug.LogWarning("MUST HAVE 2 ROOMS SELECTED");
                return;
            }
            List<VoxelVolume> volumes = new List<VoxelVolume>();
            foreach (GameObject obj in Selection.gameObjects)
            {
                VoxelVolume vol = obj.GetComponent<VoxelVolume>();
                if (vol == null)
                {
                    Debug.LogWarning(vol.name + " ISNT A VoxelVolume");
                    return;
                }
                volumes.Add(vol);
            }

            bool overlap = volumes[0].CheckOverlap(volumes[1], true);
            Debug.Log("OVERLAP: " + overlap);
        }

        [ContextMenu("Print Position")]
        public void PrintPosition()
        {
            Debug.Log(transform.position);
        }

        [ContextMenu("Toggle Visual")]
        public void ToggleVisual()
        {
            m_drawVolume = !m_drawVolume;
        }

        public bool checkPositionOverlap(VoxelVolume volume)
        {
            Vector3 ourPos = transform.position;
            Vector3 othPos = volume.transform.position;
            return ourPos == othPos;
        }

        public bool CheckOverlap(VoxelVolume volume, bool debug = false)
        {
            bool xOverlap = (volume.xMin > xMin && volume.xMin < xMax) || (volume.xMax > xMin && volume.xMax < xMax);
            bool yOverlap = (volume.yMin > yMin && volume.yMin < yMax) || (volume.yMax > yMin && volume.yMax < yMax);
            bool zOverlap = (volume.zMin > zMin && volume.zMin < zMax) || (volume.zMax > zMin && volume.zMax < zMax);
            float distance = (volume.transform.position - transform.position).magnitude + 0.00001f;
            if (debug)
            {
                Debug.Log("(" + volume.xMin + ">" + xMin + "&&" + volume.xMin + "<" + xMax + ") || (" + volume.xMax + ">" + xMin + "&&" + volume.xMax + "<" + xMax + ")");
                Debug.Log("(" + volume.yMin + ">" + yMin + "&&" + volume.yMin + "<" + yMax + ") || (" + volume.yMax + ">" + yMin + "&&" + volume.yMax + "<" + yMax + ")");
                Debug.Log("(" + volume.zMin + ">" + zMin + "&&" + volume.zMin + "<" + zMax + ") || (" + volume.zMax + ">" + zMin + "&&" + volume.zMax + "<" + zMax + ")");
                Debug.Log(xOverlap + ", " + yOverlap + ", " + zOverlap + ", " + distance + "<" + (m_size / 2) + "+" + (volume.size / 2));
            }
            return (xOverlap && yOverlap && zOverlap && distance < (m_size / 2 + volume.size / 2)) || distance < (m_size / 2 + volume.size / 2);
        }

        public static bool operator ==(VoxelVolume volumeA, VoxelVolume volumeB)
        {
            if (object.ReferenceEquals(volumeA, null))
            {
                return object.ReferenceEquals(volumeB, null);
            }
            if (object.ReferenceEquals(volumeB, null))
            {
                return object.ReferenceEquals(volumeA, null);
            }

            Vector3 aT = volumeA.transform.position;
            Vector3 bT = volumeB.transform.position;
            return aT == bT;
        }

        public static bool operator !=(VoxelVolume volumeA, VoxelVolume volumeB)
        {
            if (object.ReferenceEquals(volumeA, null))
            {
                return object.ReferenceEquals(volumeB, null) == false;
            }
            if (object.ReferenceEquals(volumeB, null))
            {
                return object.ReferenceEquals(volumeA, null) == false;
            }
            Vector3 aT = volumeA.transform.position;
            Vector3 bT = volumeB.transform.position;
            return aT != bT;
        }

        private void OnDrawGizmos()
        {
            if (m_drawVolume)
            {
#if UNITY_EDITOR
                Color gizmoColors = Gizmos.color;

                Gizmos.color = Color.white;
                Gizmos.DrawWireCube(transform.position, new Vector3(m_size, m_size, m_size));

                Gizmos.color = gizmoColors;
#endif
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            return this == obj as VoxelVolume;
        }
    }
}
