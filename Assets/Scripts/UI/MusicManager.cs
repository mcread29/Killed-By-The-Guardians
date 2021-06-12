using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

namespace UntitledFPS
{
    public class MusicManager : MonoBehaviour
    {
        AudioSource m_source;

        [SerializeField] private AudioMixer m_mixer;

        private string m_volumeParamName = "volume";

        [SerializeField] private bool m_startMenu = false;
        [SerializeField] private AudioClip m_menuMusic;
        [SerializeField] private float m_defaultMenuMusicVolume = 0.1f;
        [Space]
        [SerializeField] private bool m_startGame = false;
        [SerializeField] private AudioClip m_gameplayMusic;
        [SerializeField] private float m_defaultGameMusicVolume = 0.2f;

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

            if (m_startMenu) fadeInMenu();
            else if (m_startGame) fadeInGame(0.5f);
        }

        public void fadeInMenu(float time = 4f)
        {
            m_source.volume = 0.1f;
            m_mixer.SetFloat("musicVolume", -80f);

            m_source.clip = m_menuMusic;
            m_source.Play();

            StartCoroutine(FadeMixerGroup.StartFade(m_mixer, "musicVolume", time, 1));
        }

        public void fadeInGame(float time)
        {
            m_source.volume = 0.2f;
            m_mixer.SetFloat("musicVolume", -80f);

            m_source.clip = m_gameplayMusic;
            m_source.Play();

            StartCoroutine(FadeMixerGroup.StartFade(m_mixer, "musicVolume", time, 1));
        }

        public void fadeOutMusic(float time)
        {
            StartCoroutine(FadeMixerGroup.StartFade(m_mixer, "musicVolume", time, 0));
        }

        public void fadeOutEnemies(float time)
        {
            StartCoroutine(FadeMixerGroup.StartFade(m_mixer, "enemyVolume", time, 0));
        }

        public void resetEnemies()
        {
            m_mixer.SetFloat("enemyVolume", 0);
        }
    }
}
