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

        [SerializeField] private LayerMask m_layerToIgnore;
        [SerializeField] private Transform m_barrelParent;
        [SerializeField] private Transform m_playerTransform;

        [SerializeField] private Transform m_healthBar;

        [Range(-90, 90)]
        [SerializeField] private int m_maxRotation = 0;

        public static float JumpDropRate = 0f;
        public static JumpDrop JumpDrop;
        public static float HealthDropRate = 0f;
        public static HealthDrop HealthDrop;

        private bool m_inView = false;

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
                m_firing = false;
                if (((rotation.x + 90) % 360) - 90 > m_maxRotation)
                {
                    m_barrelParent.rotation = prevRotation;
                }
                else if (m_inView)
                {
                    m_firing = true;
                }
            }

            if (m_healthBar != null && m_playerTransform != null) m_healthBar.LookAt(m_playerTransform);

            base.Update();
        }

        private void FixedUpdate()
        {
            m_inView = false;
            RaycastHit hit;
            foreach (Barrel barrel in m_barrels)
            {
                Transform t = barrel.transform;
                if (Physics.Raycast(t.position, t.forward, out hit, Mathf.Infinity, ~m_layerToIgnore))
                {
                    m_inView = m_inView || hit.collider.gameObject.layer == m_playerTransform.gameObject.layer;
                }
            }
        }

        public void SetTransformLookat(Transform lookAt)
        {
            m_playerTransform = lookAt;
        }
    }
}
