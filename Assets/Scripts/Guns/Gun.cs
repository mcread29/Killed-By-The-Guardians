using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class Gun : Weapon
    {
        private void Awake()
        {
        }

        protected override void Update()
        {
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
