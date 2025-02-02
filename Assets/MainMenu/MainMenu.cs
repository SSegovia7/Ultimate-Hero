using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(1); // 1 is the index of the main 
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
