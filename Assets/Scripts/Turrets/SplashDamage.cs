using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    // [RequireComponent(typeof(SphereCollider))]
    public class SplashDamage : Damager
    {
        [SerializeField] private float m_delay = 1f;

        private void Awake()
        {
            StartCoroutine(delay());
        }

        private IEnumerator delay()
        {
            yield return new WaitForSeconds(m_delay);
            Destroy(GetComponent<SphereCollider>());
        }
        private void OnTriggerEnter(Collider other)
        {
            Health health = other.GetComponentInParent<Health>();
            if (other.gameObject.layer != gameObject.layer && IsInLayerMask(other.gameObject, m_damageLayer) && health != null)
            {
                health.TakeDamage(m_damage);
            }
        }
    }
}
