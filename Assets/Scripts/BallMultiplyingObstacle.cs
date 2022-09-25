using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMultiplyingObstacle : Obstacle
{
    [field: SerializeField]
    public float NewBallsLifeSeconds { get; set; }
    [field: SerializeField]
    public int NewBallsAmount { get; set; }
    [field: SerializeField]
    public bool InheritSpeed { get; set; }
    [field: SerializeField]
    public float SpeedIfNotInherited { get; set; }
    [field: SerializeField]
    public float NewBallScale { get; set; }

    public override void BallHit(Ball ball)
    {
        CreateNewBalls(ball);
        base.BallHit(ball);
    }

    private void CreateNewBalls(Ball originalBall)
    {
        for (int i = 0; i < NewBallsAmount; i++)
        {
            CreateNewBall(originalBall);
        }
    }

    private void CreateNewBall(Ball originalBall)
    {
        var newBall = GameObject.Instantiate(originalBall);
        newBall.AllowUserStart = false;
        newBall.transform.localScale *= NewBallScale;
        newBall.gameObject.AddComponent<DyingObject>();
        var dyingObject = newBall.GetComponent<DyingObject>();
        dyingObject.LifeSeconds = NewBallsLifeSeconds;
        dyingObject.transform.position = originalBall.transform.position;
        SetSpeed(newBall, originalBall);
        newBall.SetDirection(Random.insideUnitCircle);
    }

    private void SetSpeed(Ball newBall, Ball originalBall)
    {
        if (InheritSpeed)
        {
            newBall.Speed = originalBall.Speed;
            return;
        }
        newBall.Speed = SpeedIfNotInherited;
    }
}
