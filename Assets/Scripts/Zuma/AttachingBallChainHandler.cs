using Cysharp.Threading.Tasks;
using PathCreation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttachingBallChainHandler 
{
    public static event Action<int> OnMatchBall;
    public static event Action<Ball> OnInChain;
    private readonly PathCreator _pathCreator;
    private readonly BallChainConfig _ballChainConfig;
    private readonly ChainTracker _chainTracker;
    private readonly BallProvider _ballProvider;

    

    public AttachingBallChainHandler(PathCreator pathCreator, 
        BallChainConfig ballChainConfig, 
        ChainTracker chainTracker, BallProvider ballProvider)
    {
        _pathCreator = pathCreator;
        _ballChainConfig = ballChainConfig;
        _chainTracker = chainTracker;
        _ballProvider = ballProvider;
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
        OnInChain?.Invoke(newBall);
        Debug.Log("插入成功");
        //WaitToCheckAndDestroyMatches(newBall).Forget();


    }

    private async UniTask WaitToCheckAndDestroyMatches(Ball insertedBall)
    {
        await UniTask.Delay((int)(_ballChainConfig.DurationMovingOffset * 1000));
        CheckAndDestroyMatches(insertedBall);
    }
    private void CheckAndDestroyMatches(Ball insertedBall)
    {
        List<Ball> matchingBalls = new List<Ball> { insertedBall };
        Color matchColor = insertedBall.ballColor;

        //前序遍历
        Ball frontBall = insertedBall.PreviousBall;
        while (frontBall != null)
        {
            if (frontBall.ballColor == matchColor)
            {
                matchingBalls.Add(frontBall);
                frontBall = frontBall.PreviousBall;
            }else
            {
                frontBall = null;
            }
        }

        Ball backBall = insertedBall.NextBall;
        while (backBall != null)
        {
            if (backBall.ballColor == matchColor)
            {
                matchingBalls.Add(backBall);
                backBall = backBall.NextBall;
            }
            else
            {
                backBall = null;
            }
        }

        if (matchingBalls.Count >= _ballChainConfig.MatchingCount)
        {
            int count = matchingBalls.Count;
            PlayDestroyMatchingBalls(matchingBalls, matchingBalls.Count);
            
            
        }
        else
        {
            return;
        }
    }

    private void PlayDestroyMatchingBalls(List<Ball> matchingBalls, int count)
    {
        foreach (var ball in matchingBalls)
        {
            ball.PlayDestroyAnimation(() =>
            {
                if (OnMatchBall == null)
                {
                    Debug.LogError("没有订阅OnMatchBall事件");
                }
                else
                {
                    OnMatchBall?.Invoke(count);
                }
                _chainTracker.RemoveBall(ball);
                ball.Deactivate();
                _ballProvider.ReturnBall(ball);
            });
        }
        Debug.Log("消除成功");
    }



}

