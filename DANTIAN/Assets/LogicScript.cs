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

    private string highScoreKey;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "FlappyScene")
        {
            highScoreKey = "highScoreEasy";
        }
        else if (SceneManager.GetActiveScene().name == "HardFlappy")
        {
            highScoreKey = "highScoreHard";
        }
        else
        {
            Debug.LogError("Unknown game mode!");
        }

        highScoreText.text = "High Score:\n" + GetHighScore().ToString();
    }

    [ContextMenu("Increase Score")]
    public void addScore(int scoreToAdd)
    {
        score = score + scoreToAdd;
        scoreText.text = score.ToString();
        jump.Play();

        if (score > GetHighScore())
        {
            SetHighScore(score);
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
        PlayerPrefs.DeleteKey(highScoreKey);
        highScoreText.text = "High Score:\n" + GetHighScore().ToString();
        restartGame();
    }

    private int GetHighScore()
    {
        return PlayerPrefs.GetInt(highScoreKey, 0);
    }

    private void SetHighScore(int value)
    {
        PlayerPrefs.SetInt(highScoreKey, value);
    }
}
