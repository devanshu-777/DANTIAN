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
    public GameObject Guidelines;

    private Quaternion targetRotation; // The target rotation angle
    private float rotationSpeed = 50f;

    // Start is called before the first frame update
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        targetRotation = Quaternion.Euler(0f, 0f, 0f);
    }   

    // Update is called once per frame
    void Update()
    {
        if (birdIsAlive)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                myRigidbody.velocity = Vector2.up * flapStrength;
                targetRotation = Quaternion.Euler(0f, 0f, 45f); // Set the target rotation to upward angle
                transform.rotation = targetRotation; // Apply the target rotation immediately
            }
            else
            {
                targetRotation = Quaternion.Euler(0f, 0f, -45f); // Set the target rotation to downward angle
            }
        }
        if (transform.position.y < (-13) || transform.position.y > 13)
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
                Guidelines.SetActive(false);
            }
            else
            {
                Time.timeScale = 1;
                PauseMenu.SetActive(false);
                Guidelines.SetActive(true);
                escKeyCount--;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        logic.gameOver();
        birdIsAlive = false;
    }

    private void FixedUpdate()
    {
        // Gradually rotate the bird towards the target rotation
        if (!Input.GetKey(KeyCode.Space))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}
