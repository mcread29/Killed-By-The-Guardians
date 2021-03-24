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
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
                m_barrel.firing = true;
            if (Input.GetMouseButtonUp(0))
                m_barrel.Reset();
        }
    }
}
