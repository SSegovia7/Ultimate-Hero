using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string _sceneName;

    public void PlayGame()
    {
        SceneManager.LoadScene(_sceneName); // 1 is the index of the main 
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void GoToLoseScreen()
    {
        SceneManager.LoadScene("Lose Screen");
    }
    public void GoToWinScreen()
    {
        SceneManager.LoadScene("Canon Event");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
