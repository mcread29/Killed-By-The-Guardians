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

        private System.Action<GameObject, int> m_onCollisionEnter;
        private int m_damage;

        private void Awake()
        {
            m_fireTimer = 0;
        }

        private void fire()
        {
            Missile missile = GameObject.Instantiate(m_missile, transform.position, transform.rotation);
            missile.onCollisionEnter = m_onCollisionEnter;
            missile.damage = m_damage;
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
            m_damage = damage;
        }

        public void SetFireRate(float fireRate)
        {
            m_fireRate = fireRate;
            m_fireTimer = 1f / fireRate;
        }

        public void SetCollisionCallback(System.Action<GameObject, int> callback)
        {
            m_onCollisionEnter = callback;
        }

        public void Reset()
        {
            m_firing = false;
            m_fireTimer = 1f / m_fireRate;
        }
    }
}
