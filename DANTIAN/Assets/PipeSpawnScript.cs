using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawnScript : MonoBehaviour
{
    public GameObject pipe;
    private LogicScript logic;
    public float spawnRate;
    private float timer = 0;
    public float heightOffset;
    public BirdScript bird;

    // Start is called before the first frame update
    void Start()
    {
        spawnPipe();
        bird = GameObject.FindGameObjectWithTag("Bird").GetComponent<BirdScript>();
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }

    // Update is called once per frame 
    void Update()
    {
        if(timer < spawnRate)
        {
            timer = timer + Time.deltaTime;
        }
        else
        {
            if(bird.birdIsAlive){
                spawnPipe();
                timer = 0;
            }
        }
    }

    void spawnPipe()
    {
        float lowestPoint = transform.position.y - heightOffset;
        float highestPoint = transform.position.y + heightOffset;
        Instantiate(pipe, new Vector3(transform.position.x, Random.Range(lowestPoint,highestPoint),0), transform.rotation);
    }
}
