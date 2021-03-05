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
