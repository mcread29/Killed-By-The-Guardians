using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UntitledFPS
{
    [CustomEditor(typeof(Generator))]
    public class GeneratorInspector : Editor
    {
        SerializedProperty m_numAttempts;
        SerializedProperty m_data;
        SerializedProperty m_randomSeed;
        SerializedProperty m_useCustomSeed;
        Generator generator;

        void OnEnable()
        {
            generator = (Generator)target;
            m_data = serializedObject.FindProperty("m_data");
            m_randomSeed = serializedObject.FindProperty("m_randomSeed");
            m_useCustomSeed = serializedObject.FindProperty("m_useCustomSeed");
            m_numAttempts = serializedObject.FindProperty("m_numAttempts");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(m_numAttempts);
            EditorGUILayout.PropertyField(m_data);
            EditorGUILayout.PropertyField(m_useCustomSeed);
            EditorGUILayout.PropertyField(m_randomSeed);
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
