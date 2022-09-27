using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
