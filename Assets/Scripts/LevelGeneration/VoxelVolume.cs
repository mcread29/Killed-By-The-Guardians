using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class VoxelVolume : MonoBehaviour
    {
        [HideInInspector]
        [SerializeField] private float m_size = 0;

        public static VoxelVolume CreateVoxel(float size)
        {
            GameObject obj = new GameObject("VoxelVolume");
            VoxelVolume voxel = obj.AddComponent<VoxelVolume>();
            voxel.m_size = size;
            return voxel;
        }

        public static bool operator ==(VoxelVolume volumeA, VoxelVolume volumeB)
        {
            Vector3 aT = volumeA.transform.position;
            Vector3 bT = volumeB.transform.position;
            return aT.x == bT.x && aT.y == bT.y && aT.z == bT.z;
        }

        public static bool operator !=(VoxelVolume volumeA, VoxelVolume volumeB)
        {
            Vector3 aT = volumeA.transform.position;
            Vector3 bT = volumeB.transform.position;
            return aT.x != bT.x && aT.y != bT.y && aT.z != bT.z;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as VoxelVolume);
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            Color gizmoColors = Gizmos.color;

            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(transform.position, new Vector3(m_size, m_size, m_size));

            Gizmos.color = gizmoColors;
#endif
        }
    }
}
