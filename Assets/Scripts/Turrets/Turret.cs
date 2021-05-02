using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Spawnable))]
    public class Turret : Weapon
    {
        public static System.Action<Transform> SetLookAt;

        [SerializeField] private Transform m_barrelParent;
        [SerializeField] private Transform m_playerTransform;

        [Range(-90, 90)]
        [SerializeField] private int m_maxRotation = 0;

        public static float JumpDropRate = 0f;
        public static JumpDrop JumpDrop;
        public static float HealthDropRate = 0f;
        public static HealthDrop HealthDrop;

        private Health m_health;
        public Health health
        {
            get
            {
                if (m_health == null) m_health = GetComponent<Health>();
                return m_health;
            }
        }

        public bool dead { get { return health.dead; } }

        private void Awake()
        {
            health.onDeath += die;
            Turret.SetLookAt += SetTransformLookat;
        }

        void die()
        {
            //DO OTHER DEATH THINGS HERE

            float dropPercentage = Random.Range(0f, 1f);
            Debug.Log($"WILL DROP? {dropPercentage}");
            if (dropPercentage <= Turret.JumpDropRate)
            {
                if (Turret.JumpDrop != null)
                {
                    Instantiate(Turret.JumpDrop, transform.position, transform.rotation);
                }
            }
            else if (dropPercentage <= Turret.HealthDropRate + Turret.JumpDropRate)
            {
                if (Turret.HealthDrop != null)
                {
                    Instantiate(Turret.HealthDrop, transform.position, transform.rotation);
                }
            }
            gameObject.SetActive(false);
        }

        protected override void Update()
        {
            if (m_barrelParent != null && m_playerTransform != null)
            {
                Quaternion prevRotation = m_barrelParent.rotation;
                m_barrelParent.LookAt(m_playerTransform);

                Vector3 rotation = m_barrelParent.transform.localRotation.eulerAngles;
                if (((rotation.x + 90) % 360) - 90 > m_maxRotation)
                {
                    m_barrelParent.rotation = prevRotation;
                    m_firing = false;
                }
                else
                {
                    m_firing = true;
                }
            }

            base.Update();
        }

        public void SetTransformLookat(Transform lookAt)
        {
            m_playerTransform = lookAt;
        }
    }
}
