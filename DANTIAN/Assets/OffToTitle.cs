using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OffToTitle : MonoBehaviour
{
    public void OffToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 6);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 9);
    }
}
