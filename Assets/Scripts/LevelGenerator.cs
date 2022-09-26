using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class LevelGenerator : MonoBehaviour
{
    private Random _random;
    private Camera _camera;
    private ICollection<GameObject> _createdObstacles;

    [field: SerializeField]
    public Sprite SquareSprite { get; set; }

    private void Awake()
    {
        _random = new();
        _camera = Camera.main;
        _createdObstacles = new List<GameObject>();
    }

    public void GenerateLevel(Difficulty difficulty)
    {
        _createdObstacles.Clear();
        var obstaclesAmount = GetObstaclesAmount(difficulty); ;
        var obstacles = Enumerable.Range(0, obstaclesAmount).Select(i => CraeteRandomObstacle(difficulty)).ToList();
        obstacles.ForEach(CreateObstacleObject);
    }

    private Type CraeteRandomObstacle(Difficulty difficulty)
    {
        return new RandomObstacleGenerator(difficulty).GenerateObstacle();
    }

    private int GetObstaclesAmount(Difficulty difficulty)
    {
        return new ObstaclesAmountProvider().GetAmount(difficulty);
    }

    private void CreateObstacleObject(Type type)
    {
        var obj = new GameObject($"obstacle-{Guid.NewGuid()}");
        obj.transform.localScale = new Vector3((float)_random.Next(1, 10) / 10, (float)_random.Next(1, 10) / 10, 0);
        obj.AddComponent<BoxCollider2D>();
        var spriteRenderer = obj.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = SquareSprite;
        obj.AddComponent(type);
    }
}

public enum Difficulty
{
    Easy, Middle, Hard
}

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

public class RandomObstacleGenerator
{
    private IEnumerable<Type> _helpingObstacles;
    private IEnumerable<Type> _neutralObstacles;
    private IEnumerable<Type> _disturbingObstacles;
    private readonly Random _random;
    private readonly int _helpingProbability;
    private readonly int _neutralProbability;
    private readonly int _disturbingProbability;

    public RandomObstacleGenerator(Difficulty difficulty)
    {
        SegregateObstacles();
        _random = new();
        (_helpingProbability,
         _neutralProbability,
         _disturbingProbability) = GetProbability(difficulty);
    }

    private void SegregateObstacles()
    {
        var helping = new List<Type>();
        var neutral = new List<Type>();
        var disturbing = new List<Type>();

        foreach (var obstacle in GetAllObstacleTypes())
        {
            var type = RecognizeType(obstacle);
            switch (type)
            {
                case ObstacleType.Helping:
                    helping.Add(obstacle);
                    break;
                case ObstacleType.Neutral:
                    neutral.Add(obstacle);
                    break;
                case ObstacleType.Disturbing:
                    disturbing.Add(obstacle);
                    break;
            }
        }

        _helpingObstacles = helping;
        _neutralObstacles = neutral;
        _disturbingObstacles = disturbing;
    }



    private (int helping, int neutral, int disturbing) GetProbability(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                return (30, 50, 20);
            case Difficulty.Middle:
                return (15, 70, 15);
            case Difficulty.Hard:
                return (20, 50, 30);
            default:
                throw new NotImplementedException($"Unknown level difficulty: {difficulty}. Unable to generate level.");
        }
    }

    private ObstacleType RecognizeType(Type obstacle)
    {
        CheckObstacleTypeValid(obstacle);
        var obstacleAttribute = obstacle.GetCustomAttribute<ObstacleAttribute>();

        if (obstacleAttribute is null)
        {
            return ObstacleType.Neutral;
        }

        return obstacleAttribute.Type;
    }

    private void CheckObstacleTypeValid(Type obstacle)
    {
        if (obstacle is null)
        {
            throw new ArgumentNullException(nameof(obstacle));
        }

        if (!typeof(Obstacle).IsAssignableFrom(obstacle))
        {
            throw new InvalidOperationException("Trying to get obstacle type of type which does not inherite after Obstacle and is not Obstacle type.");
        }
    }

    private IEnumerable<Type> GetAllObstacleTypes()
    {
        var assembly = Assembly.GetExecutingAssembly();
        return assembly.GetTypes().Where(t => typeof(Obstacle).IsAssignableFrom(t));
    }

    public Type GenerateObstacle()
    {
        var randomNumber = _random.Next(1, _helpingProbability + _neutralProbability + _disturbingProbability);

        if (randomNumber <= _helpingProbability)
        {
            return GetHelpingObstacle();
        }

        if (randomNumber <= _neutralProbability)
        {
            return GetNeutralObstacle();
        }

        return GetDisturbingObstacle();
    }

    private Type GetHelpingObstacle()
    {
        return GetRadnomElement(_helpingObstacles);
    }

    private Type GetRadnomElement(IEnumerable<Type> types)
    {
        var idx = _random.Next(0, types.Count() - 1);
        return types.ElementAt(idx);
    }

    private Type GetNeutralObstacle()
    {
        return GetRadnomElement(_neutralObstacles);
    }

    private Type GetDisturbingObstacle()
    {
        return GetRadnomElement(_disturbingObstacles);
    }
}