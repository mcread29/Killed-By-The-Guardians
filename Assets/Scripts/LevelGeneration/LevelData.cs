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

    [System.Serializable]
    public class RoomSceneData
    {
        public RoomSceneData(Room volume, RoomName room, Vector3 position)
        {
            m_room = volume;
            m_roomName = room;
            m_position = position;
        }

        [SerializeField] private Room m_room;
        public Room room { get { return m_room; } }

        [SerializeField] private RoomName m_roomName;
        public RoomName roomName { get { return m_roomName; } }

        private Vector3 m_position;
        public Vector3 position
        {
            get { return m_position; }
            set { m_position = value; }
        }
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

        [SerializeField] private RoomSceneData[] m_startRooms;
        public RoomSceneData[] startRooms { get { return m_startRooms; } }

        [SerializeField] private RoomSceneData[] m_availableRooms;
        public RoomSceneData[] availableRooms { get { return m_availableRooms; } }

        [SerializeField] private RoomSceneData[] m_endingRooms;
        public RoomSceneData[] endingRooms { get { return m_endingRooms; } }

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
