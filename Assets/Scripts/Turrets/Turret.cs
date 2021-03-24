using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    [RequireComponent(typeof(Health))]
    public class Turret : MonoBehaviour
    {
        [SerializeField] private Transform m_barrelParent;
        [SerializeField] private Transform m_playerTransform;

        [SerializeField] private GunData m_data;
        [SerializeField] private Barrel m_barrel;

        private float m_fireTimer = 0;

        private Health m_health;

        private void Awake()
        {
            m_health = GetComponent<Health>();
            m_health.onDeath += die;

            m_barrel.SetDamage(m_data.damage);
            m_barrel.SetFireRate(m_data.fireRate);
        }

        void die()
        {
            //DO OTHER DEATH THINGS HERE
            gameObject.SetActive(false);
        }

        void Update()
        {
            if (m_barrelParent != null && m_playerTransform != null)
            {
                Quaternion prevRotation = m_barrelParent.rotation;
                m_barrelParent.LookAt(m_playerTransform);
                Vector3 rotation = m_barrelParent.transform.localRotation.eulerAngles;
                if (rotation.x < 270 || rotation.y > 360)
                {
                    m_barrelParent.rotation = prevRotation;
                }
                else
                {
                    m_barrel.firing = true;
                }
            }
        }
    }
}
