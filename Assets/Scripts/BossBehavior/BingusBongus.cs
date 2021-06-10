using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class BingusBongus : bOSS
    {
        private Rigidbody m_rigidBody;

        [SerializeField] private GameObject m_shield;

        [SerializeField] private ShieldGenerator m_1stGenerator;
        [SerializeField] private RoomSection m_1stSection;
        [Space]

        [SerializeField] private ShieldGenerator m_2ndGenerator;
        [SerializeField] private RoomSection m_2ndSection;
        [Space]

        [SerializeField] private ShieldGenerator m_3rdGenerator;
        [SerializeField] private RoomSection m_3rdSection;
        [Space]

        [SerializeField] private ShieldGenerator m_4thGenerator;
        [SerializeField] private RoomSection m_4thSection;

        private int m_stage = 0;
        private int m_activeGenerators = 1;

        private bool m_started = false;

        [SerializeField] private RectTransform m_healthBar;

        private void nextStage()
        {
            m_stage++;
            m_activeGenerators = 0;
            if (m_stage >= 5)
            {
                //GAME OVER BOIS
                m_1stGenerator.gameObject.SetActive(false);
                m_2ndGenerator.gameObject.SetActive(false);
                m_3rdGenerator.gameObject.SetActive(false);
                m_4thGenerator.gameObject.SetActive(false);
                gameObject.SetActive(false);
                m_bossKilled = true;
                if (onBossKilled != null) onBossKilled();
            }
            else
            {
                GoTweenConfig config = new GoTweenConfig();
                config.anchoredPosition(new Vector3(0, 25, 0));
                Go.to(m_healthBar, 0.25f, config);

                if (m_stage >= 1)
                {
                    m_1stGenerator.Activate();
                    m_1stSection.startSection();
                    m_activeGenerators++;
                }
                if (m_stage >= 2)
                {
                    m_2ndGenerator.Activate();
                    m_2ndSection.startSection();
                    m_activeGenerators++;
                }
                if (m_stage >= 3)
                {
                    m_3rdGenerator.Activate();
                    m_3rdSection.startSection();
                    m_activeGenerators++;
                }
                if (m_stage >= 4)
                {
                    m_4thGenerator.Activate();
                    m_4thSection.startSection();
                    m_activeGenerators++;
                }

                m_shield.SetActive(true);
                m_health.enabled = false;
                m_health.FullHeal();
            }
        }

        private void generatorDestroyed()
        {
            if (m_stage >= 5) return;

            m_activeGenerators--;
            if (m_activeGenerators == 0)
            {
                m_health.enabled = true;
                m_shield.SetActive(false);

                GoTweenConfig config = new GoTweenConfig();
                config.anchoredPosition(new Vector3(0, -65, 0));
                Go.to(m_healthBar, 0.25f, config);
            }
        }

        public override void StartBoss()
        {
            if (m_started) return;

            m_started = true;

            m_rigidBody = GetComponent<Rigidbody>();
            m_health = GetComponent<Health>();
            m_health.enabled = false;
            m_health.onDeath += nextStage;

            System.Action<float, float> hit = null;
            hit = (float h, float mh) => UI.Instance.HitEnemy();
            m_health.updateHealth += hit;

            m_1stGenerator.SetLineEnd(transform);
            m_1stGenerator.generatorDestroyed = generatorDestroyed;

            m_2ndGenerator.SetLineEnd(transform);
            m_2ndGenerator.generatorDestroyed = generatorDestroyed;

            m_3rdGenerator.SetLineEnd(transform);
            m_3rdGenerator.generatorDestroyed = generatorDestroyed;

            m_4thGenerator.SetLineEnd(transform);
            m_4thGenerator.generatorDestroyed = generatorDestroyed;

            m_rigidBody.velocity = new Vector3(20, 20, 20);
            nextStage();
        }
    }
}
