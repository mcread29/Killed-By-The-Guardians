using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] protected GunData m_data;
        [SerializeField] protected Barrel[] m_barrels;

        protected bool m_firing = false;
        protected float m_fireTimer = 0;

        protected virtual void Update()
        {
            m_fireTimer += Time.deltaTime;
            if (m_firing)
            {
                if (m_fireTimer >= 1 / m_data.fireRate)
                {
                    m_fireTimer = 0;
                    foreach (Barrel barrel in m_barrels)
                    {
                        barrel.Fire(m_data);
                    }
                }
            }
        }
    }
}
