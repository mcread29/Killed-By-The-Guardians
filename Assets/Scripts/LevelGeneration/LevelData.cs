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
        [SerializeField] private float m_turretHealthDropRate = 0.06f;
        public float turretHealthDropRate { get { return m_turretHealthDropRate; } }

        [SerializeField] private HealthDrop m_healthDropPrefab;
        public HealthDrop healthDropPrefab { get { return m_healthDropPrefab; } }

        // [SerializeField] private JumpDrop m_jumpDropPrefab;
        // public JumpDrop jumpDropPrefab { get { return m_jumpDropPrefab; } }

        [SerializeField] private float m_turretJumpDropRate = 0.01f;
        public float turretJumpDropRate { get { return m_turretJumpDropRate; } }

        [SerializeField] private int m_minLength;
        public int minLength { get { return m_minLength; } }

        [SerializeField] private int m_maxLength;
        public int maxLength { get { return m_maxLength; } }

        [Range(0, 1)]
        [SerializeField] private float m_straightness = 1;
        public float straightness { get { return m_straightness; } }

        [Range(1, 5)]
        [SerializeField] private int m_branches;
        public int branches { get { return m_branches; } }

        [SerializeField] private RoomSceneRoot[] m_startRooms;
        public RoomSceneRoot[] startRooms { get { return m_startRooms; } }

        [SerializeField] private RoomSceneRoot[] m_availableRooms;
        public RoomSceneRoot[] availableRooms { get { return m_availableRooms; } }

        [SerializeField] private RoomSceneRoot[] m_endingRooms;
        public RoomSceneRoot[] endingRooms { get { return m_endingRooms; } }

#if UNITY_EDITOR
        [MenuItem("Assets/Create/LevelData")]
        public static void CreateMyAsset()
        {
            LevelData asset = ScriptableObject.CreateInstance<LevelData>();

            AssetDatabase.CreateAsset(asset, "Assets/LevelData/NewScripableObject.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
#endif
    }
}
