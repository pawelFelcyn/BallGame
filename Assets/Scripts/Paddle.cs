using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Paddle : MonoBehaviour
{
    private Rigidbody2D _thisRigidbody;

    [field: SerializeField]
    public float Speed { get; set; }
    [field: SerializeField]
    public float MinXPosition { get; set; }
    [field: SerializeField]
    public float MaxXPosition { get; set; }

    private void Start()
    {
        _thisRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        UserControl();
    }

    private void UserControl()
    {
        if (MustStop())
        {
            return;
        }
        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _thisRigidbody.velocity = Vector2.left * Speed;
            return;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            _thisRigidbody.velocity = Vector2.right * Speed;
            return;
        }

        Stop();
    }

    private bool MustStop()
    {
        if (transform.position.x < MinXPosition)
        {
            Stop();
            transform.position = new Vector2(MinXPosition, transform.position.y);
            return true;
        }

        if (transform.position.x > MaxXPosition)
        {
            Stop();
            transform.position = new Vector2(MaxXPosition, transform.position.y);
            return true;
        }

        return false;
    }

    private void Stop()
    {
        _thisRigidbody.velocity = Vector2.zero;
    }
}
