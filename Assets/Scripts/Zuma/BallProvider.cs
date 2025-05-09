using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallProvider
{
    private int _initialCount = 10;
    private GameObject _ball;
    private List<Ball> _allBall;
    private BallChainController _controller;
    private BallChainConfig _config;
    private Queue<Ball> _inactiveBalls = new Queue<Ball>();

    public BallProvider(GameObject ball, BallChainController controller, BallChainConfig config)
    {
        _ball = ball;
        _controller = controller;
        _config = config;
        _initialCount = (int)Math.Round(controller.pathCreator.path.length / config.ZumaBallRadius) + 10;
    }

    public void CreatePoolBall()
    {
        _allBall = new List<Ball>();

        for (int i = 0; i < _initialCount; i++)
        {
            var ball = CreateObject(Vector3.zero, Quaternion.identity);
            ball.Deactivate();
        }
    }

    

    public void CleanupPool()
    {
        foreach (var ball in _allBall.Where(ball => ball != null))
        {
            UnityEngine.Object.Destroy(ball.gameObject);
        }
        _inactiveBalls.Clear();

        _allBall.Clear();
    }

    public Ball GetBall(Vector3 position, Quaternion rotation)
    {
        if (_inactiveBalls.Count > 0)
        {
            Ball ball = _inactiveBalls.Dequeue();
            ball.Activate(position, rotation);
            return ball;
        }
        else
        {
            Ball newBall = CreateObject(position, rotation);
            _allBall.Add(newBall);
            return newBall;
        }
    }

    public void ReturnBall(Ball ball)
    {
        _inactiveBalls.Enqueue(ball);
        ball.ResetState();
        ball.Deactivate();

    }

    private Ball CreateObject(Vector3 position, Quaternion rotation)
    {
        var createdObject = _controller.Create(_ball, position, rotation).GetComponent<Ball>();
        _inactiveBalls.Enqueue(createdObject);
        _allBall.Add(createdObject);
        return createdObject;
    }

    public List<Ball> GetPool()
    {
        return _allBall;
    }
}
