using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class Projectile : Damager
    {
        [Tooltip("Lifespan in seconds")]
        [SerializeField] private int m_maxLifespan = 60;
        private float m_lifeTime = 0;

        [SerializeField] private GameObject m_hitExplosion;

        private Rigidbody m_rigidBody;

        private void Awake()
        {
            m_rigidBody = GetComponent<Rigidbody>();
            m_lifeTime = 0;
        }

        private void Update()
        {
            m_lifeTime += Time.deltaTime;
            if (m_lifeTime >= m_maxLifespan) Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, transform.forward, out hit);

            Health health = other.GetComponentInParent<Health>();
            if (other.gameObject.layer != gameObject.layer)
            {
                if (IsInLayerMask(other.gameObject, m_damageLayer) && health != null)
                {
                    health.TakeDamage(m_damage);
                }
                Destroy(gameObject);
                if (m_hitExplosion != null)
                {

                    GameObject impactP = Instantiate(m_hitExplosion, transform.position, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;
                    impactP.layer = gameObject.layer;
                    impactP.GetComponent<Damager>().SetData(m_damage, m_damageLayer);
                    Destroy(impactP, 5.0f);
                }
            }
        }
    }
}