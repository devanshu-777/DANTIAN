using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlappyMenu : MonoBehaviour
{
    [Scene] public FlappyMenu menu;
    public void FlappyEasy()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void FlappyHard()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    public void ReturnMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
