using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    // [RequireComponent(typeof(SphereCollider))]
    public class SplashDamage : Damager
    {
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
