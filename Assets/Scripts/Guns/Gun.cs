using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private GunData m_data;
        [SerializeField] private Barrel m_barrel;

        private bool m_firing = false;

        private float m_fireTimer = 0;

        private void Awake()
        {
            m_barrel.SetDamage(m_data.damage);
            m_barrel.SetFireRate(m_data.fireRate);

            m_barrel.SetCollisionCallback(TriggerHit);
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
                m_barrel.firing = true;
            if (Input.GetMouseButtonUp(0))
                m_barrel.Reset();
        }

        private void TriggerHit(GameObject other, int damage)
        {
            Health health = other.GetComponentInParent<Health>();
            if (IsInLayerMask(other, m_data.damageLayer) && health != null)
            {
                Debug.Log("WOWZA: " + other.layer + " | " + m_data.damageLayer + " | " + damage);
                health.TakeDamage(damage);
            }
        }

        public bool IsInLayerMask(GameObject obj, LayerMask layerMask)
        {
            return ((layerMask.value & (1 << obj.layer)) > 0);
        }
    }
}
