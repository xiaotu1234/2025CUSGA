using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBallTracker
{
    private float _thresholdToPoint = 0.35f;
    private float _bufferOutOfScreen = 0.1f;

    private Ball _ball;
    private Coroutine _intersectionTrackerCoroutine;
    private BallChainController _controller;
}
