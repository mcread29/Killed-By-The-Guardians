using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class Gun : Weapon
    {

        private static bool m_active = true;
        public static bool active { get { return m_active; } }

        public static void Activate()
        {
            m_active = true;
        }

        public static void Deactivate()
        {
            m_active = false;
        }

        protected override void Update()
        {
            if (m_active == false) return;

            if (Input.GetMouseButtonDown(0))
                m_firing = true;
            if (Input.GetMouseButtonUp(0))
            {
                m_firing = false;
                // m_fireTimer = 1f / m_data.fireRate;
            }
            base.Update();
        }
    }
}
