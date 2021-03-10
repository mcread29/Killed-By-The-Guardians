using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UntitledFPS
{
    [System.Serializable]
    public class WeightedItem<T>
    {
        [SerializeField] private T m_item;
        [SerializeField] private float m_weight;
    }

    [System.Serializable]
    public class WeightedList<T>
    {
        [SerializeField] private WeightedItem<T>[] m_items;
    }

    public class LevelData : ScriptableObject
    {
        [SerializeField] private int m_minLength;
        public int minLength { get { return m_minLength; } }

        [SerializeField] private int m_maxLength;
        public int maxLength { get { return m_maxLength; } }

        [Range(0, 1)]
        [SerializeField] private float m_straightness = 1;
        public float straightness { get { return m_straightness; } }

        [SerializeField] private Room[] m_startRooms;
        public Room[] startRooms { get { return m_startRooms; } }

        [SerializeField] private Room[] m_availableRooms;
        public Room[] availableRooms { get { return m_availableRooms; } }

        [SerializeField] private Room[] m_endingRooms;
        public Room[] endingRooms { get { return m_endingRooms; } }

        [MenuItem("Assets/Create/LevelData")]
        public static void CreateMyAsset()
        {
            LevelData asset = ScriptableObject.CreateInstance<LevelData>();

            AssetDatabase.CreateAsset(asset, "Assets/LevelData/NewScripableObject.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }
}
