using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class Damager : MonoBehaviour
    {
        protected int m_damage;
        protected LayerMask m_damageLayer;

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
