using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallProvider
{
    private int _initialCount = 10;
    private GameObject _ball;
    private List<Ball> _pool;
    private BallChainController _controller;
    private BallChainConfig _config;

    public BallProvider(GameObject ball, BallChainController controller, BallChainConfig config)
    {
        _ball = ball;
        _controller = controller;
        _config = config;
        _initialCount = (int)Math.Round(controller.pathCreator.path.length / config.ZumaBallRadius) + 2;
    }

    public void CreatePoolBall()
    {
        _pool = new List<Ball>();

        for (int i = 0; i < _initialCount; i++)
        {
            var ball = CreateObject(Vector3.zero, Quaternion.identity);
            ball.Deactivate();
        }
    }

    public void CleanupPool()
    {
        foreach (var ball in _pool.Where(ball => ball != null))
        {
            UnityEngine.Object.Destroy(ball.gameObject);
        }

        _pool.Clear();
    }

    public Ball GetBall(Vector3 position, Quaternion rotation)
    {
        foreach (var ball in _pool)
        {
            if (!ball.gameObject.activeInHierarchy)
            {
                ball.Activate(position, rotation);
                return ball;
            }
        }

        Ball newBall = CreateObject(position, rotation);
        newBall.Activate(position, rotation);
        return newBall;
    }

    public void ReturnBall(Ball ball)
    {
        ball.Deactivate();
    }

    private Ball CreateObject(Vector3 position, Quaternion rotation)
    {
        var createdObject = _controller.Create(_ball, position, rotation).GetComponent<Ball>();
        _pool.Add(createdObject);
        return createdObject;
    }
}
