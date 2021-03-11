using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UntitledFPS
{
    [CustomEditor(typeof(Generator))]
    public class GeneratorInspector : Editor
    {
        SerializedProperty m_numRooms;
        SerializedProperty m_data;
        Generator generator;

        void OnEnable()
        {
            generator = (Generator)target;
            m_data = serializedObject.FindProperty("m_data");
            m_numRooms = serializedObject.FindProperty("m_numRooms");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(m_numRooms);
            EditorGUILayout.PropertyField(m_data);
            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Generate"))
            {
                generator.Clear();
                generator.Generate();
            }
            if (GUILayout.Button("Clear"))
            {
                generator.Clear();
            }
        }
    }
}
