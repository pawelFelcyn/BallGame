using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DyingObject : MonoBehaviour
{
    private float _lives;

    [field: SerializeField]
    public float LifeSeconds { get; set; } = 15;

    private void Start()
    {
        _lives = 0;
    }

    private void Update()
    {
        if (_lives >= LifeSeconds)
        {
            Die();
        }

        _lives += Time.deltaTime;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
