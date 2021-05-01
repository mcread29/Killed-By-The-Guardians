using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UntitledFPS
{
    public class ScreenTransition : MonoBehaviour
    {
        public static ScreenTransition m_instance;
        public static ScreenTransition Instance { get { return m_instance; } }

        [SerializeField] private RectTransform m_transitionImage;

        private void Awake()
        {
            Debug.Log(m_transitionImage.anchoredPosition);
            Debug.Log(m_transitionImage.rect.size);
            if (m_instance == null)
            {
                m_instance = this;
                m_transitionImage.anchoredPosition = new Vector2(m_transitionImage.rect.size.x, 0);
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public static void To(string sceneName)
        {
            m_instance.m_transitionImage.anchoredPosition = new Vector2(m_instance.m_transitionImage.rect.size.x, 0);
            GoTweenConfig config = new GoTweenConfig().anchoredPosition(Vector2.zero).setEaseType(GoEaseType.SineIn);
            config.onCompleteHandler += (AbstractGoTween t) =>
            {
                SceneManager.LoadSceneAsync(sceneName);
                SceneManager.sceneLoaded += m_instance.sceneLoaded;
            };
            Go.to(m_instance.m_transitionImage, 0.5f, config);
        }

        private void sceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= sceneLoaded;
            StartCoroutine(afterSceneLoaded(scene));
        }

        private IEnumerator afterSceneLoaded(Scene scene)
        {
            while (Generator.Instance.finishedGenerating == false) yield return new WaitForEndOfFrame();
            Debug.Log("GENERATRO DOEN");
            Go.to(m_instance.m_transitionImage, 0.5f, new GoTweenConfig().anchoredPosition(new Vector2(-m_instance.m_transitionImage.rect.size.x, 0)).setEaseType(GoEaseType.SineIn));
        }
    }
}
