using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LogicScript : MonoBehaviour
{
    public int score;
    public Text scoreText;
    public Text highScoreText;
    public GameObject gameOverScreen;
    public AudioSource jump;
    public AudioSource gameEnd;
    public int gameOverCount;

    void Start()
    {
        highScoreText.text = "High Score:\n" + PlayerPrefs.GetInt("highScore", 0).ToString();
    }

    [ContextMenu("Increase Score")]
    public void addScore(int scoreToAdd)
    {
        score = score + scoreToAdd;
        scoreText.text = score.ToString();
        jump.Play();

        if (score > PlayerPrefs.GetInt("highScore", 0))
        {
            PlayerPrefs.SetInt("highScore", score);
            highScoreText.text = "High Score:\n" + score.ToString();
        }
    }

    public void restartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void returnEasyMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void returnHardMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }

    public void gameOver()
    {
        gameOverCount++;
        gameOverScreen.SetActive(true);
        if (gameOverCount == 1)
        {
            gameEnd.Play();
        }
    }

    public void resetScore()
    {
        PlayerPrefs.DeleteKey("highScore");
        restartGame();
    }
}
