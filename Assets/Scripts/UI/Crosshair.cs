using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class Crosshair : MonoBehaviour
    {
        [SerializeField] private RectTransform m_top;
        [SerializeField] private RectTransform m_bottom;
        [SerializeField] private RectTransform m_right;
        [SerializeField] private RectTransform m_left;

        [SerializeField] private float m_space = 10;
        [SerializeField] private float m_width = 10;
        [SerializeField] private float m_length = 20;

        private void Awake()
        {
            m_top.position.Set(0, m_space, 0);
            m_bottom.position.Set(0, -m_space, 0);
            m_right.position.Set(m_space, 0, 0);
            m_left.position.Set(-m_space, 0, 0);
        }
    }
}
