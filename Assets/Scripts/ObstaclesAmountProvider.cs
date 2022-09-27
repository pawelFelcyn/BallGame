using System;
using Random = System.Random;

public class ObstaclesAmountProvider
{
    private readonly Random _random;

    public ObstaclesAmountProvider()
    {
        _random = new Random();
    }

    public int GetAmount(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                return EasyObstaclesAmount();
            case Difficulty.Middle:
                return MiddleObstacleAmount();
            case Difficulty.Hard:
                return HardObstacleAmount();
            default:
                throw new NotImplementedException($"Unknown level difficulty: {difficulty}. Unable to generate level.");
        }
    }

    private int EasyObstaclesAmount()
    {
        return _random.Next(20, 35);
    }

    private int MiddleObstacleAmount()
    {
        return _random.Next(36, 60);
    }

    private int HardObstacleAmount()
    {
        return _random.Next(60, 100);
    }
}
