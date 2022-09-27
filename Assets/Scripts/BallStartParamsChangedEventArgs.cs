using UnityEngine;

public class BallStartParamsChangedEventArgs
{
    public BallStartParamsChangedEventArgs(float maxLength, Vector2 newDirection)
    {
        MaxLength = maxLength;
        NewDirection = newDirection;
    }

    public float MaxLength { get; }
    public Vector2 NewDirection { get; }
}

public delegate void BallStartParamsChangedEvent(BallStartParamsChangedEventArgs args);
