using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class Barrel : MonoBehaviour
    {
        [SerializeField] private Missile m_missile;

        private bool m_firing = false;
        public bool firing
        {
            get { return m_firing; }
            set { m_firing = value; }
        }

        private float m_fireTimer;
        private float m_fireRate;

        private void Awake()
        {
            m_fireTimer = 0;
        }

        private void fire()
        {
            GameObject.Instantiate(m_missile, transform.position, transform.rotation);
        }

        private void Update()
        {
            if (m_firing)
            {
                m_fireTimer += Time.deltaTime;
                if (m_fireTimer >= 1 / m_fireRate)
                {
                    m_fireTimer = 0;
                    fire();
                }
            }
        }

        public void SetDamage(int damage)
        {
            m_missile.damage = damage;
        }

        public void SetFireRate(float fireRate)
        {
            m_fireRate = fireRate;
            m_fireTimer = 1f / fireRate;
        }

        public void Reset()
        {
            m_firing = false;
            m_fireTimer = 1f / m_fireRate;
        }
    }
}
