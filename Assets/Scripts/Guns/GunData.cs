using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class GunData : ScriptableObject
    {
        [SerializeField] private float m_fireRate;
        [SerializeField] private float m_damage;
    }
}
