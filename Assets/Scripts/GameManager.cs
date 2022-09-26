using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<LevelGenerator>().GenerateLevel(Difficulty.Hard);
    }
}
