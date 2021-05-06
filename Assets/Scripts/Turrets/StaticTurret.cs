using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    [RequireComponent(typeof(Spawnable))]
    public class StaticTurret : Weapon
    {
        [SerializeField] private float m_fireDelay = 0;

        private void Awake()
        {
            m_firing = true;
        }

        public void StopFiring()
        {
            m_firing = false;
        }

        protected override void Update()
        {
            if (m_fireDelay < 0)
            {
                base.Update();
            }
            else
            {
                m_fireDelay -= Time.deltaTime;
            }
        }
    }
}
