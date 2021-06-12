using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

namespace UntitledFPS
{
    public class MusicManager : MonoBehaviour
    {
        AudioSource m_source;

        private string m_volumeParamName = "volume";

        [SerializeField] private AudioClip m_menuMusic;
        [SerializeField] private AudioClip m_gameplayMusic;

        private static MusicManager m_instance;
        public static MusicManager Instance { get { return m_instance; } }

        private void Awake()
        {
            if (m_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            m_instance = this;

            DontDestroyOnLoad(this);
            m_source = GetComponent<AudioSource>();
        }

        public void fadeInMenu()
        {
            m_source.volume = 0;
            m_source.clip = m_menuMusic;
            m_source.Play();
            StartCoroutine(FadeAudioSource.StartFade(m_source, 4f, 0.1f));
        }

        public void fadeOutMusic()
        {

        }
    }
}
