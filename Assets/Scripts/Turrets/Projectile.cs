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

        private void FixedUpdate()
        {
            RaycastHit hit;
            float dist = m_rigidBody.velocity.magnitude * Time.deltaTime;
            Vector3 dir = transform.GetComponent<Rigidbody>().velocity;
            if (transform.GetComponent<Rigidbody>().useGravity)
                dir += Physics.gravity * Time.deltaTime;
            dir = dir.normalized;

            if (Physics.SphereCast(transform.position, 0.01f, dir, out hit, dist))
            {
                Health health = hit.transform.GetComponentInParent<Health>();
                if (hit.transform.gameObject.layer != gameObject.layer)
                {
                    if (IsInLayerMask(hit.transform.gameObject, m_damageLayer) && health != null)
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

        private void OnTriggerEnter(Collider other)
        {
        }
    }
}