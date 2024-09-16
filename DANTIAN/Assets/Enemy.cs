using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject _explodePrefab;

    public void Die()
    {
        Instantiate(_explodePrefab, transform.position, Quaternion.Euler(90, 0, 0));
        Destroy(gameObject);
    }
}
