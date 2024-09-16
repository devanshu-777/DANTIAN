using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScript : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public float flapStrength;
    public LogicScript logic;
    public bool birdIsAlive = true;
    private int escKeyCount = 0;
    public GameObject PauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }   

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && birdIsAlive)
        {
            myRigidbody.velocity = Vector2.up * flapStrength;
        }
        if(transform.position.y < (-13) || transform.position.y > 13)
        {
            logic.gameOver();
            birdIsAlive = false;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(escKeyCount == 0)
            {
                Time.timeScale = 0;
                escKeyCount++;
                PauseMenu.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                PauseMenu.SetActive(false);
                escKeyCount--;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        logic.gameOver();
        birdIsAlive = false;
    }
}
