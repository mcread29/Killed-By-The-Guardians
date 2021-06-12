using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace UntitledFPS
{
    public class WinScreen : MonoBehaviour
    {
        [SerializeField] private CanvasGroup m_winText;
        [SerializeField] private Text m_timeText;
        [SerializeField] private CanvasGroup m_creditsText;
        [SerializeField] private CanvasGroup m_thanksText;

        public void Show(float time)
        {
            MusicManager.Instance.fadeOutEnemies(1.5f);
            StartCoroutine(fadeMusic());

            TimeSpan span = TimeSpan.FromSeconds(time);
            m_timeText.text = span.ToString(@"mm\:ss\:ff");

            GoTweenConfig config = new GoTweenConfig();

            CanvasGroup group = GetComponent<CanvasGroup>();
            ActionTweenProperty p = new ActionTweenProperty(0, 1, (val) => group.alpha = val);
            config.addTweenProperty(p);
            config.onComplete((t) => StartCoroutine(hideWinText()));

            Go.to(this, 2f, config);
        }

        private IEnumerator fadeMusic()
        {
            MusicManager.Instance.fadeOutMusic(0.85f);
            yield return new WaitForSeconds(0.85f);
            MusicManager.Instance.fadeInGame(0.5f);
        }

        private IEnumerator hideWinText()
        {
            yield return new WaitForSeconds(5);

            GoTweenConfig config = new GoTweenConfig();

            ActionTweenProperty p = new ActionTweenProperty(1, 0, (val) => m_winText.alpha = val);
            config.addTweenProperty(p);
            config.onComplete((t) => showCredits());

            Go.to(this, 2f, config);
        }

        private void showCredits()
        {
            GoTweenConfig config = new GoTweenConfig();

            ActionTweenProperty p = new ActionTweenProperty(0, 1, (val) => m_creditsText.alpha = val);
            config.addTweenProperty(p);
            config.onComplete((t) => StartCoroutine(hideCredits()));

            Go.to(this, 2f, config);
        }

        private IEnumerator hideCredits()
        {
            yield return new WaitForSeconds(5);

            GoTweenConfig config = new GoTweenConfig();

            ActionTweenProperty p = new ActionTweenProperty(1, 0, (val) => m_creditsText.alpha = val);
            config.addTweenProperty(p);
            config.onComplete((t) => showThanks());

            Go.to(this, 2f, config);
        }

        private void showThanks()
        {
            GoTweenConfig config = new GoTweenConfig();

            ActionTweenProperty p = new ActionTweenProperty(0, 1, (val) => m_thanksText.alpha = val);
            config.addTweenProperty(p);
            config.onComplete((t) => StartCoroutine(hideThanks()));

            Go.to(this, 2f, config);
        }

        private IEnumerator hideThanks()
        {
            yield return new WaitForSeconds(5);

            GoTweenConfig config = new GoTweenConfig();

            ActionTweenProperty p = new ActionTweenProperty(1, 0, (val) => m_creditsText.alpha = val);
            config.addTweenProperty(p);
            config.onComplete((t) => UI.Instance.PlayerKilled());

            Go.to(this, 2f, config);
        }
    }
}
