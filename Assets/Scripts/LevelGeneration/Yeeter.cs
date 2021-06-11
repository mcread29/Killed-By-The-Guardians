using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class Yeeter : MonoBehaviour
    {
        [SerializeField] private float m_yeetForce = 1500;

        private AudioSource m_audioSource;
        [SerializeField] private Clip m_clip;

        private void Awake()
        {
            m_audioSource = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("WOWZA");
            if (other.tag == "Player")
            {
                Debug.Log("YEET");
                other.transform.parent.GetComponent<FPSController.PlayerMovement>().ForceJump(m_yeetForce);
                m_clip.Play(m_audioSource);
            }
        }
    }
}
