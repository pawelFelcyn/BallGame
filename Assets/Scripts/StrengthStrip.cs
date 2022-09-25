using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthStrip : MonoBehaviour
{
    private Transform _meshTransform;

    private void Start()
    {
        SubscribeEvents(GameObject.Find("FreeScripts").GetComponent<BallStarter>());
        _meshTransform = transform.Find("StrengthStripMesh");
        SetStartScale();
        transform.position = GameObject.Find("MainBall").transform.position;
    }

    private void SubscribeEvents(BallStarter starter)
    {
        starter.StartParamsChanged += UpdateStrip;
        starter.BallStarted += () => Destroy(gameObject);
        starter.KickingCanceled += SetStartScale;
    }
    
    private void SetStartScale()
    {
        _meshTransform.localScale = WithChangedY(_meshTransform.localScale, 0);
    }

    private Vector3 WithChangedY(Vector3 original, float newY)
    {
        return new Vector3(original.x, newY, original.z);
    }

    private void UpdateStrip(BallStartParamsChangedEventArgs args)
    {
        ScaleMesh(args);
        Rotate(args.NewDirection);
    }

    private void ScaleMesh(BallStartParamsChangedEventArgs args)
    {
        var newY = Mathf.Clamp(args.NewDirection.magnitude, 0, args.MaxLength);
        _meshTransform.localScale = WithChangedY(_meshTransform.localScale, newY);
        _meshTransform.localPosition = WithChangedY(_meshTransform.localPosition, -(newY / 2));
    }

    private void Rotate(Vector2 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction) * Quaternion.FromToRotation(Vector3.right, Vector3.forward);
        transform.Rotate(0, 0, -90);
    }
}
