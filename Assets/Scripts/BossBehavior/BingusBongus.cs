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
        [SerializeField] private ShieldGenerator m_2ndGenerator;
        [SerializeField] private ShieldGenerator m_3rdGenerator;
        [SerializeField] private ShieldGenerator m_4thGenerator;

        private int m_stage = 1;
        private int m_activeGenerators = 1;
        private Health m_health;

        private void Awake()
        {
        }

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
                if (bossKilled != null) bossKilled();
            }
            else
            {
                if (m_stage >= 1)
                {
                    m_1stGenerator.Activate();
                    m_activeGenerators++;
                }
                if (m_stage >= 2)
                {
                    m_2ndGenerator.Activate();
                    m_activeGenerators++;
                }
                if (m_stage >= 3)
                {
                    m_3rdGenerator.Activate();
                    m_activeGenerators++;
                }
                if (m_stage >= 4)
                {
                    m_4thGenerator.Activate();
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
            }
        }

        public override void StartBoss()
        {
            gameObject.SetActive(true);

            m_rigidBody = GetComponent<Rigidbody>();
            m_health = GetComponent<Health>();
            m_health.enabled = false;
            m_health.onDeath += nextStage;

            m_1stGenerator.SetLineEnd(transform);
            m_1stGenerator.generatorDestroyed = generatorDestroyed;

            m_2ndGenerator.SetLineEnd(transform);
            m_2ndGenerator.generatorDestroyed = generatorDestroyed;

            m_3rdGenerator.SetLineEnd(transform);
            m_3rdGenerator.generatorDestroyed = generatorDestroyed;

            m_4thGenerator.SetLineEnd(transform);
            m_4thGenerator.generatorDestroyed = generatorDestroyed;

            m_rigidBody.velocity = new Vector3(20, 20, 20);
            m_1stGenerator.Activate();
        }
    }
}
