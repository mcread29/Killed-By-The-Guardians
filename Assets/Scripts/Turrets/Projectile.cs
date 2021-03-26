using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class Projectile : MonoBehaviour
    {
        [Tooltip("Lifespan in seconds")]
        [SerializeField] private int m_maxLifespan = 60;
        private float m_lifeTime = 0;

        [SerializeField] private GameObject m_hitExplosion;

        private Rigidbody m_rigidBody;
        private int m_damage;
        private LayerMask m_damageLayer;

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
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                Debug.Log("Point of contact: " + hit.point);
            }
            Debug.Log(other.name);

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
                    Destroy(impactP, 5.0f);
                }
            }
        }

        public bool IsInLayerMask(GameObject obj, LayerMask layerMask)
        {
            return ((layerMask.value & (1 << obj.layer)) > 0);
        }

        public void SetData(GunData data)
        {
            m_damage = data.damage;
            m_damageLayer = data.damageLayer;
        }
    }
}