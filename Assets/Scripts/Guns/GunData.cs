using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UntitledFPS
{
    public enum FireType
    {
        PROJECTILE,
        HIT_SCAN
    }

    public class GunData : ScriptableObject
    {
        [SerializeField] private FireType m_fireType;
        public FireType fireType { get { return m_fireType; } }

        [SerializeField] private float m_fireRate;
        public float fireRate { get { return m_fireRate; } }

        [SerializeField] private int m_damage;
        public int damage { get { return m_damage; } }

        [MenuItem("Assets/Create/GunData")]
        public static void CreateMyAsset()
        {
            GunData asset = ScriptableObject.CreateInstance<GunData>();

            AssetDatabase.CreateAsset(asset, "Assets/Guns/GunData.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }
}
