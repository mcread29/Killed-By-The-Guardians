using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class StaticTurret : Weapon
    {
        [SerializeField] private float m_fireDelay = 0;

        private void Awake()
        {
            m_firing = true;
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

        public void Spawn()
        {
            //TEMP REPLACE WITH OTHER STUFF
            gameObject.SetActive(true);
        }
    }
}
