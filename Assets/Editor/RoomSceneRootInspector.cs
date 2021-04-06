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

        private SerializedProperty m_lighting;
        private SerializedProperty m_room;
        private SerializedProperty m_staticObjects;
        private SerializedProperty m_player;

        private void OnEnable()
        {
            root = (RoomSceneRoot)target;
            m_lighting = serializedObject.FindProperty("m_lighting");
            m_room = serializedObject.FindProperty("m_room");
            m_staticObjects = serializedObject.FindProperty("m_staticObjects");
            m_player = serializedObject.FindProperty("m_player");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(m_lighting);
            EditorGUILayout.PropertyField(m_room);
            EditorGUILayout.PropertyField(m_staticObjects);
            EditorGUILayout.PropertyField(m_player);

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Bake Lighting"))
            {
                root.BakeLighting();
            }
        }
    }
}
