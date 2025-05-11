using PathCreation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttachingBallChainHandler 
{
    private readonly PathCreator _pathCreator;
    private readonly BallChainConfig _ballChainConfig;
    private readonly ChainTracker _chainTracker;

    

    public AttachingBallChainHandler(PathCreator pathCreator, 
        BallChainConfig ballChainConfig, 
        ChainTracker chainTracker)
    {
        _pathCreator = pathCreator;
        _ballChainConfig = ballChainConfig;
        _chainTracker = chainTracker;
    }
    public void TryAttachBall(Ball newBall)
    {
        var collision = GetClosestCollision(newBall);
        if (collision == null)
            return ;

        InsertBallToChain(newBall, collision);
    }
    private Ball GetClosestCollision(Ball newBall)
    {
        var path = _pathCreator.path;
        float minDistance = float.MaxValue;
        Ball closest = null;
        Ball currentBall = _chainTracker.Balls.FirstOrDefault();


        foreach(var ball in _chainTracker.Balls)
        {
            float distance = Vector3.Distance(ball.transform.position, newBall.transform.position);
            if (distance < _ballChainConfig.CollisionThreshold && distance < minDistance)
            {
                closest = ball;
             

             
                minDistance = distance;
            }
        }

        return closest;
    }

    private void InsertBallToChain(Ball newBall, Ball collision)
    {
        var path = _pathCreator.path;
        float newBallDist = path.GetClosestDistanceAlongPath(newBall.transform.position);
        float closestBallDist = path.GetClosestDistanceAlongPath(collision.transform.position);
        if (collision.NextBall == null && newBallDist < closestBallDist)
        {
            _chainTracker.AddBallLast(newBall);
        }
        else if (collision.PreviousBall == null && newBallDist > closestBallDist)
        {
            _chainTracker.AddBallFirst(newBall);
        }
        else
        {
           _chainTracker.InsertBall(newBall, collision);
        }
        
        _chainTracker.AddChainHeadDistance(_ballChainConfig.SpacingBalls);
        _chainTracker.AddBallLast(newBall);
    }





}

