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

        [SerializeField] private LayerMask m_damageLayer;
        public LayerMask damageLayer { get { return m_damageLayer; } }

        [SerializeField] private Projectile m_projectile;
        public Projectile projectile { get { return m_projectile; } }

        [SerializeField] private GameObject m_muzzleFlash;
        public GameObject muzzleFlash { get { return m_muzzleFlash; } }

        [SerializeField] private GameObject m_hitExplosion;
        public GameObject hitExplosion { get { return m_hitExplosion; } }

        [SerializeField] private float m_shotSpeed;
        public float shotSpeed { get { return m_shotSpeed; } }

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
