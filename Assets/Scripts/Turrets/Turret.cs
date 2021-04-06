using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    [RequireComponent(typeof(Health))]
    public class Turret : Weapon
    {
        [SerializeField] private Transform m_barrelParent;
        [SerializeField] private Transform m_playerTransform;

        [Range(-90, 90)]
        [SerializeField] private int m_maxRotation = 0;

        private Health m_health;

        private void Awake()
        {
            m_health = GetComponent<Health>();
            m_health.onDeath += die;
        }

        void die()
        {
            //DO OTHER DEATH THINGS HERE
            Debug.Log("DIE DIE DIE");
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

        public void SetLookat(Transform lookAt)
        {
            m_playerTransform = lookAt;
        }
    }
}
