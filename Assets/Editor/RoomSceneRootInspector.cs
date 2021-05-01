using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UntitledFPS
{
    [CustomEditor(typeof(RoomSceneRoot))]
    public class RoomSceneRootInspector : Editor
    {
        RoomSceneRoot root;

        private SerializedProperty m_room;
        private SerializedProperty m_staticObjects;
        private SerializedProperty m_player;
        private SerializedProperty m_sections;

        private void OnEnable()
        {
            root = (RoomSceneRoot)target;
            m_room = serializedObject.FindProperty("m_room");
            m_staticObjects = serializedObject.FindProperty("m_staticObjects");
            m_player = serializedObject.FindProperty("m_player");
            m_sections = serializedObject.FindProperty("m_sections");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(m_room);
            EditorGUILayout.PropertyField(m_staticObjects);
            EditorGUILayout.PropertyField(m_player);
            EditorGUILayout.PropertyField(m_sections);

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Bake Lighting"))
            {
                root.BakeLighting();
            }
        }
    }
}
