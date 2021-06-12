using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace UntitledFPS
{
    public class Splash : MonoBehaviour
    {
        [SerializeField] private CanvasGroup m_splashImage;

        // Start is called before the first frame update
        void Start()
        {
            MusicManager.Instance.fadeInMenu();
            MainMenu.previousScene = SceneManager.GetActiveScene().name;
            StartCoroutine(goToMenu());
        }

        private IEnumerator goToMenu()
        {
            yield return new WaitForSeconds(5f);
            GoTweenConfig config = new GoTweenConfig();

            ActionTweenProperty p = new ActionTweenProperty(1, 0, (val) => m_splashImage.alpha = val);
            config.addTweenProperty(p);
            config.onComplete((t) => SceneManager.LoadScene("TempMenu"));

            Go.to(this, 2f, config);
        }
    }
}
