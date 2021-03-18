using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class Missile : MonoBehaviour
    {
        [SerializeField] private float m_speed;
        private Vector3 m_Position = Vector3.zero;

        private Rigidbody m_rigidBody;

        private void Awake()
        {
            m_rigidBody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            m_rigidBody.velocity = transform.forward * m_speed;
        }
    }
}
