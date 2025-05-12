using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBallTracker: MonoBehaviour
{
    private float _thresholdToPoint = 0.35f;
    private float _bufferOutOfScreen = 0.1f;

    public Ball _ball;
    private Coroutine _intersectionTrackerCoroutine;
    public BallChainController _controller;

    private void OnEnable()
    {
        _controller = BallChainController.Instance;
        _ball = GetComponent<Ball>();
    }


}
