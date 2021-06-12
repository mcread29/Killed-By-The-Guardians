using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UntitledFPS
{
    public class MainMenu : MonoBehaviour
    {
        public static string previousScene = "";
        private void Start()
        {
            if (previousScene == "Splash")
            {
                fadeIn();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) StartLevel();
        }

        private void fadeIn()
        {
            CanvasGroup group = GetComponent<CanvasGroup>();
            group.alpha = 0;

            GoTweenConfig config = new GoTweenConfig();
            ActionTweenProperty p = new ActionTweenProperty(0, 1, (val) => group.alpha = val);
            config.addTweenProperty(p);

            Go.to(this, 2f, config);
        }

        public void StartLevel()
        {
            ScreenTransition.To("LevelGeneration");
        }
    }
}
