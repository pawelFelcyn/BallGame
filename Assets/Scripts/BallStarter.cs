using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallStarter : MonoBehaviour
{
    private float _maxStrength;
    private bool _holdsBall = false;
    private bool _ballKicked = false;
    private Camera _camera;
    private Ball _mainBall;
    private float _mainBallRadius;
    private Vector2 _direction;
    [SerializeField]
    private float _strengthRate = 5;
    [SerializeField]
    private float _minSlope = 0.3f;
    private float _strength;
    private void Start()
    {
        _camera = Camera.main;
        _mainBall = GameObject.Find("MainBall").GetComponent<Ball>();
        _mainBallRadius = _mainBall.GetComponent<CircleCollider2D>().radius;
        _maxStrength = _mainBall.MaxSpeed;
    }

    private void Update()
    {
        UserPlays();
    }

    private void UserPlays()
    {
        if (_ballKicked)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            TryChatchBall();
        }

        if (Input.GetMouseButtonDown(1))
        {
            CancelKick();
        }

        if (Input.GetMouseButton(0))
        {
            CalculateKickParams();
        }

        if (Input.GetMouseButtonUp(0))
        {
            KickBallIfHolds();
        }
    }

    private void TryChatchBall()
    {
        var mouseWorldPosition = GetMouseWorldPosition();
        if (IsBallCaught(mouseWorldPosition))
        {
            _holdsBall = true;
        }
    }

    private bool IsBallCaught(Vector2 otherPoint)
    {
        return Vector2.Distance(_mainBall.transform.position, otherPoint) < _mainBallRadius;
    }

    private Vector2 GetMouseWorldPosition()
    {
        var position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        return _camera.ScreenToWorldPoint(position);
    }

    private void CancelKick()
    {
        _direction = Vector2.zero;
        _strength = 0;
        _holdsBall = false;
        KickingCanceled?.Invoke();
    }

    private void CalculateKickParams()
    {
        if (!_holdsBall)
        {
            _direction = Vector2.zero;
            _strength = 0;
            return;
        }

        var mouseWorldPosition = GetMouseWorldPosition();
        _direction = Vector3ToVector2(_mainBall.transform.position) - mouseWorldPosition;
        
        var slope = CalculateSlope(_direction);
        if (slope < _minSlope)
        {
            CancelKick();
            return;
        }

        _strength = _direction.magnitude * _strengthRate;
        _strength = Math.Clamp(_strength, 0, _maxStrength);
        StartParamsChanged?.Invoke(new BallStartParamsChangedEventArgs(_maxStrength / _strengthRate, _direction));
    }

    private Vector2 Vector3ToVector2(Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.y);
    }

    private float CalculateSlope(Vector2 vector)
    {
        if (vector.x == 0)
        {
            return float.MaxValue;
        }

        var x = Math.Abs(vector.x);

        try
        {
            return checked(vector.y / x);
        }
        catch (OverflowException)
        {
            return float.MaxValue;
        }
    }

    private void KickBallIfHolds()
    {
        if (!_holdsBall)
        {
            return;
        }

        _mainBall.Speed = _strength;
        _mainBall.SetDirection(_direction);
        _ballKicked = true;
        BallStarted?.Invoke();
    }

    public event BallStartParamsChangedEvent StartParamsChanged;
    public event Action BallStarted;
    public event Action KickingCanceled;
}
