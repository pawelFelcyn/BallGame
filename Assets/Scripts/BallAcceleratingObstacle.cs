using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obstacle(ObstacleType.Disturbing)]
public class BallAccelerateObstacle : Obstacle
{
    private Ball _lastHitBall;
    private float _elapsed;
    private bool _currentlyAccelerates = false;
    private float _originalSpeed;
    [field: SerializeField]
    public float SecondsOfAcceleration { get; set; } = 5;

    private void Update()
    {
        if (!_currentlyAccelerates)
        {
            return;
        }

        if (_elapsed >= SecondsOfAcceleration)
        {
            _lastHitBall.Speed = _originalSpeed;
            return;
        }

        _elapsed += Time.deltaTime;
    }

    public override void BallHit(Ball ball)
    {
        _elapsed = 0;
        _originalSpeed = ball.Speed;
       _lastHitBall = ball;
        _currentlyAccelerates = true;
        ball.Speed *= 2;

        base.BallHit(ball);
    }
}
