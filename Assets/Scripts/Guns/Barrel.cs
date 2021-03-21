using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class Barrel : MonoBehaviour
    {
        [SerializeField] private Missile m_missile;

        public void Fire()
        {
            GameObject.Instantiate(m_missile, transform.position, transform.rotation);
        }
    }
}
