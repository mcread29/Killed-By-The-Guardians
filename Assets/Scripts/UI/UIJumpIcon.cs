using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class UIJumpIcon : MonoBehaviour
    {
        [SerializeField] private RectTransform m_icon;

        public void Jump()
        {
            GoTweenConfig config = new GoTweenConfig();
            config.anchoredPosition(new Vector3(0, 40, 0));
            config.setEaseType(GoEaseType.QuadOut);

            Go.to(m_icon, 0.2f, config);
        }

        public void Reset()
        {
            GoTweenConfig config = new GoTweenConfig();
            config.anchoredPosition(new Vector3(0, 0, 0));
            config.setEaseType(GoEaseType.QuadOut);

            Go.to(m_icon, 0.2f, config);
        }
    }
}
