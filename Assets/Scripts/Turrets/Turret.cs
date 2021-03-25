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

        private Health m_health;

        private void Awake()
        {
            m_health = GetComponent<Health>();
            m_health.onDeath += die;

            m_barrel.SetDamage(m_data.damage);
            m_barrel.SetFireRate(m_data.fireRate);

            m_barrel.SetCollisionCallback(TriggerHit);
        }

        void die()
        {
            //DO OTHER DEATH THINGS HERE
            Debug.Log("DIE DIE DIE");
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

        private void TriggerHit(GameObject other, int damage)
        {
            if (other.layer == m_data.damageLayer && other.GetComponent<Health>() != null)
            {
                Debug.Log("WOWZA: " + other.name + ", " + other.layer);
                other.GetComponent<Health>().TakeDamage(damage);
            }
        }
    }
}
