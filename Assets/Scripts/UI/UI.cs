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
        [SerializeField] private Text m_emenyCountText;
        private int m_enemyCount = 0;
        [SerializeField] private CanvasGroup m_deathScreen;
        [SerializeField] private UIJumpCounter m_jumpcounter;

        private void Awake()
        {
            if (m_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            m_instance = this;
        }

        public void AddEnemies(int enemies)
        {
            m_enemyCount += enemies;
            m_emenyCountText.text = m_enemyCount.ToString();
            // PLAY SOUND ADDING ENEMIES
        }

        public void HitEnemy()
        {
            m_crosshair.Hit();
            // PLAY HIT SOUND
        }

        public void KillEnemy()
        {
            m_enemyCount--;
            m_emenyCountText.text = m_enemyCount.ToString();
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
