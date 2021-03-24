using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class Player : MonoBehaviour
    {
        private FPSController.PlayerMovement m_movement;
        private Health m_health;

        private void Awake()
        {
            m_movement = GetComponent<FPSController.PlayerMovement>();
            m_health = GetComponent<Health>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
