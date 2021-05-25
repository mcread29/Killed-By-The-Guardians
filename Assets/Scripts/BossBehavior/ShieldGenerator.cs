using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class ShieldGenerator : MonoBehaviour
    {
        private Transform m_boss;

        private LineRenderer m_line;
        private Health m_health;
        private Vector3[] m_points;

        public System.Action generatorDestroyed;

        private void Awake()
        {
            m_line = GetComponent<LineRenderer>();
            m_health = GetComponent<Health>();
            m_health.onDeath += shieldBroke;
        }

        private void shieldBroke()
        {
            gameObject.SetActive(false);
            if (generatorDestroyed != null) generatorDestroyed();
        }

        public void Activate()
        {
            Debug.Log($"{name} ACTIVATED");
            gameObject.SetActive(true);
            m_health.FullHeal();
        }

        public void SetLineEnd(Transform end)
        {
            m_boss = end;
        }

        // Start is called before the first frame update
        void Start()
        {
            m_points = new Vector3[]
            {
                transform.position,
                transform.position
            };
        }

        // Update is called once per frame
        void Update()
        {
            if (m_boss != null)
                m_points[1] = m_boss.transform.position;
            m_line.SetPositions(m_points);
        }
    }
}