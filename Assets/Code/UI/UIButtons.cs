using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtons : MonoBehaviour
{
    public static event Action OnQuitButton;
    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        OnQuitButton?.Invoke();
    }
    
    public void QuitGameImmediately()
    {
        Application.Quit();
    }
}