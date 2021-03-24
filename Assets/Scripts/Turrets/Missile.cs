using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class Missile : MonoBehaviour
    {
        [Tooltip("Lifespan in seconds")]
        [SerializeField] private int m_maxLifespan = 60;

        private float m_lifeTime = 0;

        [SerializeField] private float m_speed;
        private Vector3 m_Position = Vector3.zero;

        private Rigidbody m_rigidBody;

        private int m_damage = 0;
        public int damage
        {
            get { return m_damage; }
            set { m_damage = value; }
        }

        private void Awake()
        {
            m_rigidBody = GetComponent<Rigidbody>();
            m_lifeTime = 0;
        }

        private void Update()
        {
            m_rigidBody.velocity = transform.forward * m_speed;
            m_lifeTime += Time.deltaTime;
            if (m_lifeTime >= m_maxLifespan) Destroy(gameObject);
        }
    }
}