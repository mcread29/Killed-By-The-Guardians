using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public enum FadeType
    {
        IN,
        IN_OUT
    }

    public class Fader : MonoBehaviour
    {
        [SerializeField] private FadeType m_fadeType = FadeType.IN;

        private CanvasGroup m_group;

        private void Awake()
        {
            m_group = GetComponent<CanvasGroup>();
        }

        void Start()
        {
            GoTweenConfig config = new GoTweenConfig();

            ActionTweenProperty p = new ActionTweenProperty(0, 1, (val) => m_group.alpha = val);
            config.addTweenProperty(p);

            if (m_fadeType == FadeType.IN) { }
            if (m_fadeType == FadeType.IN_OUT)
            {
                config.loopType = GoLoopType.PingPong;
                config.iterations = -1;
            }

            Go.to(this, 2f, config);
        }
    }
}
