using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Ball : MonoBehaviour
{
    private Rigidbody2D _thisRigidbody;
    private Vector2 _velocityThatHasBeenSet;

    private float _speed;
    public float Speed 
    {
        get => _speed;
        set 
        {
            _speed = value;
            SetDirection(_thisRigidbody.velocity);
        }
    }
    [field: SerializeField]
    public float MaxSpeed { get; set; }
    public bool AllowUserStart { get; set; } = true;
    [field: SerializeField]
    public bool IsSpeedLimited { get; set; }
    
    private void Awake()
    {
        _thisRigidbody = GetComponent<Rigidbody2D>();
    }
   
    public void SetDirection(Vector2 velocity)
    {
        velocity.Normalize();
        _thisRigidbody.velocity = velocity * Speed;
        _velocityThatHasBeenSet = _thisRigidbody.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var normal = collision.contacts[0].normal;
        var newVelocity = Vector2.Reflect(_velocityThatHasBeenSet, normal);
        SetDirection(newVelocity);

        if (collision.gameObject.TryGetComponent<Obstacle>(out var obstacle))
        {
            HandleObstacle(obstacle);
        }
    }

    private void HandleObstacle(Obstacle obstacle)
    {
        obstacle.BallHit(this);
    }
}
