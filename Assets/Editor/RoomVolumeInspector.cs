using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UntitledFPS
{
    [CustomEditor(typeof(RoomVolume))]
    public class RoomVolumeInspector : Editor
    {
        SerializedProperty m_roomSize;
        SerializedProperty m_roomScale;
        SerializedProperty m_voxelParent;

        RoomVolume volume;

        void OnEnable()
        {
            volume = (RoomVolume)target;
            m_roomSize = serializedObject.FindProperty("m_roomSize");
            m_roomScale = serializedObject.FindProperty("m_roomScale");
            m_voxelParent = serializedObject.FindProperty("m_voxelParent");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(m_roomSize);
            EditorGUILayout.PropertyField(m_roomScale);
            EditorGUILayout.PropertyField(m_voxelParent);
            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Populate"))
            {
                volume.Clear();
                volume.PopulateVoxels();
            }
            if (GUILayout.Button("Clear"))
            {
                volume.Clear();
            }
        }
    }
}
