using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UntitledFPS
{
    public class MainMenu : MonoBehaviour
    {
        public void StartLevel()
        {
            ScreenTransition.To("LevelGeneration");
            // SceneManager.LoadScene("LevelGeneration", LoadSceneMode.Single);
        }
    }
}
