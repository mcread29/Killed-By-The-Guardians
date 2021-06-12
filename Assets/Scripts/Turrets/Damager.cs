using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class Damager : MonoBehaviour
    {
        [SerializeField] private int m_overrideDamage = -1;
        [SerializeField] private LayerMask m_overrideLayer = -1;
        protected int m_damage;
        protected LayerMask m_damageLayer;

        private void Awake()
        {
            if (m_overrideDamage != -1)
                m_damage = m_overrideDamage;
            if (m_overrideLayer != -1)
                m_damageLayer = m_overrideLayer;
        }

        public virtual void SetData(GunData data)
        {
            m_damage = data.damage;
            m_damageLayer = data.damageLayer;
        }

        public void SetData(int damage, LayerMask damageLayer)
        {
            m_damage = damage;
            m_damageLayer = damageLayer;
        }

        public bool IsInLayerMask(GameObject obj, LayerMask layerMask)
        {
            return ((layerMask.value & (1 << obj.layer)) > 0);
        }
    }
}
