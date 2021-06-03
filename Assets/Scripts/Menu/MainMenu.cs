using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UntitledFPS
{
    public class MainMenu : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) StartLevel();
        }

        public void StartLevel()
        {
            ScreenTransition.To("LevelGeneration");
        }
    }
}
