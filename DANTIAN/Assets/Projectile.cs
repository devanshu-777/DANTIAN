using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _speed;
    private PipeSpawnScript pipe;
    private BirdScript bird;

    void Start()
    {
        bird = GameObject.FindGameObjectWithTag("Bird").GetComponent<BirdScript>();
        pipe = GameObject.FindGameObjectWithTag("Breakable").GetComponent<PipeSpawnScript>();
    }

    public void Init(Vector2 dir)
    {
        _rb.velocity =  dir * _speed;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.transform.gameObject.tag == "Breakable" && bird.birdIsAlive)
        {
            col.transform.gameObject.SetActive(false);
        }
        Destroy(gameObject);
    }
}
