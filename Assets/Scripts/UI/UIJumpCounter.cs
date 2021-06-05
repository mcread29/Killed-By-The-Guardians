using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class UIJumpCounter : MonoBehaviour
    {
        [SerializeField] private UIJumpIcon m_jumpIconPrefab;
        [SerializeField] private List<UIJumpIcon> m_icons;

        private int m_jumpIndex = 0;

        public void Jump()
        {
            m_icons[m_jumpIndex].Jump();
            m_jumpIndex++;
        }

        public void Reset()
        {
            m_jumpIndex = 0;
            foreach (UIJumpIcon icon in m_icons)
                icon.Reset();
        }

        public void AddJump()
        {
            m_icons.Add(Instantiate(m_jumpIconPrefab, transform.position, transform.rotation, transform));
        }
    }
}
