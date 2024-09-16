using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        NetworkManagerHUD hud = FindObjectOfType<NetworkManagerHUD>();

        // If the NetworkManagerHUD exists, destroy it
        if (hud != null)
        {
            Destroy(hud.gameObject);
        }
    }

    public void BirdGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void Game2048()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 4);
    }

    public void PongGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 5);
    }

    public void ExtraGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 6);
    }

    public void Chat()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 9);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
