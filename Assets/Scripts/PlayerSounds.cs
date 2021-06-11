using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    [System.Serializable]
    public class Clip
    {
        [SerializeField] private AudioClip m_clip;
        [SerializeField] private float m_volume = 1f;

        public void Play(AudioSource source)
        {
            source.PlayOneShot(m_clip, m_volume);
        }
    }

    public class PlayerSounds : MonoBehaviour
    {
        private AudioSource m_audioSource;
        private void Awake()
        {
            m_audioSource = GetComponent<AudioSource>();
        }

        [SerializeField] private Clip m_jumpClip;
        public void Jump()
        {
            m_jumpClip.Play(m_audioSource);
        }

        [SerializeField] private Clip m_spawnClip;
        public void Spawn()
        {
            m_spawnClip.Play(m_audioSource);
        }

        [SerializeField] private Clip m_nextRoomClip;
        public void NextRoom()
        {
            m_nextRoomClip.Play(m_audioSource);
        }

        [SerializeField] private Clip m_healthClip;
        public void HealthPickup()
        {
            m_healthClip.Play(m_audioSource);
        }

        [SerializeField] private Clip m_jumpPackClip;
        public void JumpPickup()
        {
            m_jumpPackClip.Play(m_audioSource);
        }

        [SerializeField] private Clip m_killClip;
        public void Kill()
        {
            m_killClip.Play(m_audioSource);
        }
    }
}
