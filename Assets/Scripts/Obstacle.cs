using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private SpriteRenderer _meshRenderer;
    private float _elapsedFromLastHit;
    private bool _isMaterialOriginal = true;

    [field: SerializeField]
    public int Endurance { get; private set; }
    [field: SerializeField]
    public float MaterialChangeTimeSeconds { get; set; }
    [field: SerializeField]
    public Material HitMaterial { get; set; }
    [field: SerializeField]
    public Material OriginaMaterial { get; set; }

    private void Start()
    {
        _meshRenderer = GetComponent<SpriteRenderer>();
        _meshRenderer.material = OriginaMaterial;
    }

    private void Update()
    {
        HandleRestoringMaterial();
    }

    private void HandleRestoringMaterial()
    {
        if (_isMaterialOriginal)
        {
            return;
        }
        
        _elapsedFromLastHit += Time.deltaTime;
        
        if (_elapsedFromLastHit >= MaterialChangeTimeSeconds)
        {
            _meshRenderer.material = OriginaMaterial;
            _isMaterialOriginal = true;
        }
    }

    public virtual void BallHit(Ball ball)
    {
        _elapsedFromLastHit = 0;
        Endurance--;
        
        if (Endurance == 0)
        {
            Destroy(gameObject);
            return;
        }

        if (_isMaterialOriginal)
        {
            _meshRenderer.material = HitMaterial;
            _isMaterialOriginal = false;
        }
    }
}
