using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        
    }

    public void BirdGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void Game2048()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 4);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
