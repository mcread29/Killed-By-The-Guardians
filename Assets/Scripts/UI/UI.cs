using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UntitledFPS
{
    public class UI : MonoBehaviour
    {
        private static UI m_instance;
        public static UI Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new UI();
                }
                return m_instance;
            }
        }

        [SerializeField] private Crosshair m_crosshair;
        [SerializeField] private Text m_enemyCountText;
        private int m_enemyCount = 0;
        [SerializeField] private CanvasGroup m_deathScreen;
        [SerializeField] private UIJumpCounter m_jumpcounter;
        [SerializeField] private WinScreen m_winScreen;

        private GoTweenChain m_enemyCountScale;

        private void Awake()
        {
            if (m_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            m_instance = this;
        }

        private void Start()
        {
            m_enemyCountScale = new GoTweenChain();

            GoTweenConfig scaleOutConfig = new GoTweenConfig();
            scaleOutConfig.scale(2);
            scaleOutConfig.setEaseType(GoEaseType.QuadOut);

            GoTweenConfig scaleBackConfig = new GoTweenConfig();
            scaleBackConfig.scale(1);

            scaleBackConfig.setEaseType(GoEaseType.QuadIn);

            m_enemyCountScale.append(new GoTween(m_enemyCountText.rectTransform, 0.25f, scaleOutConfig));
            m_enemyCountScale.append(new GoTween(m_enemyCountText.rectTransform, 0.35f, scaleBackConfig));
        }

        public void AddEnemies(int enemies)
        {
            m_enemyCount += enemies;
            m_enemyCountText.text = m_enemyCount.ToString();

            m_enemyCountScale.goToAndPlay(0);
        }

        public void HitEnemy()
        {
            m_crosshair.Hit();
            // PLAY HIT SOUND
        }

        public void KillEnemy()
        {
            m_enemyCount--;
            m_enemyCountText.text = m_enemyCount.ToString();
            // PLAY ENEMY KILL SOUND
            // MAYBE PLAY SOUND WHEN <= 5?
        }

        public void PlayerKilled()
        {
            GoTweenConfig config = new GoTweenConfig();

            ActionTweenProperty p = new ActionTweenProperty(0, 1, (val) => m_deathScreen.alpha = val);
            config.addTweenProperty(p);
            config.onComplete((t) => SceneManager.LoadSceneAsync("TempMenu"));

            Go.to(this, 2f, config);
        }

        public void PlayerWon(float time)
        {
            GoTweenConfig config = new GoTweenConfig();

            CanvasGroup group = m_winScreen.GetComponent<CanvasGroup>();
            ActionTweenProperty p = new ActionTweenProperty(0, 1, (val) => group.alpha = val);
            config.addTweenProperty(p);
            config.onComplete((t) => m_winScreen.Show(time));

            Go.to(this, 2f, config);
        }

        public void Jump()
        {
            m_jumpcounter.Jump();
        }

        public void JumpReset()
        {
            m_jumpcounter.Reset();
        }

        public void AddJump()
        {
            m_jumpcounter.AddJump();
        }
    }
}
