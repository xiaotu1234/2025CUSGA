using Cysharp.Threading.Tasks;
using PathCreation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttachingBallChainHandler 
{
    public float colorTolerance = 0.005f;
    public static event Action<int> OnMatchBall;
    private readonly PathCreator _pathCreator;
    private readonly BallChainConfig _ballChainConfig;
    private readonly ChainTracker _chainTracker;
    private readonly BallProvider _ballProvider;
    private  Ball _currentShootBall;
    private Ball _frontBallEnd;
    private Ball _backBallEnd;
    private Color _matchColor;


    public AttachingBallChainHandler(PathCreator pathCreator, 
        BallChainConfig ballChainConfig, 
        ChainTracker chainTracker, BallProvider ballProvider)
    {
        _pathCreator = pathCreator;
        _ballChainConfig = ballChainConfig;
        _chainTracker = chainTracker;
        _ballProvider = ballProvider;
    }
    private float ColorDistance(Color a, Color b)
    {
        return Mathf.Sqrt(
            Mathf.Pow(a.r - b.r, 3) +
            Mathf.Pow(a.g - b.g, 3) +
            Mathf.Pow(a.b - b.b, 3)
        );
    }

    public bool TryAttachBall(Ball newBall)
    {
        _matchColor = newBall.ballColor;
        var collision = GetClosestCollision(newBall);
        if (collision == null)
        {
            Debug.LogWarning("����ʧ�ܣ���ײ������Ϊnull");
            return false;
        }
        if (ColorDistance(collision.ballColor, _matchColor) > colorTolerance)
        {
            Debug.LogWarning($"����ʧ�ܣ���ɫ���쳬���ݲ�");
            return false;
        }
        return InsertBallToChain(newBall, collision);
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

    private bool InsertBallToChain(Ball newBall, Ball collision)
    {
        
        
        _currentShootBall = newBall;
        return CheckAndDestroyMatches(collision);


    }

    private async UniTask WaitToCheckAndDestroyMatches(Ball collision)
    {
        await UniTask.Delay((int)(_ballChainConfig.DurationMovingOffset * 1000));
        CheckAndDestroyMatches(collision);
    }
    private bool CheckAndDestroyMatches(Ball collision)
    {
        Debug.Log("��ʼ������ɫ��ͬ������");
        List<Ball> matchingBalls = new List<Ball> { collision };
        if (collision != null)
        {
            _backBallEnd = collision;
            _frontBallEnd = collision;
        }

        //ǰ�����
        Ball frontBall = collision.PreviousBall;
        
        while (frontBall != null)
        {
            

            if (ColorDistance(frontBall.ballColor, _matchColor) <= colorTolerance)
            {
                matchingBalls.Add(frontBall);
                _frontBallEnd = frontBall;
                frontBall = frontBall.PreviousBall;
            }else
            {
                break;
            }
        }
        
        Ball backBall = collision.NextBall;
        
        while (backBall != null)
        {
            if (ColorDistance(backBall.ballColor, _matchColor) <= colorTolerance)
            {
                matchingBalls.Add(backBall);
                _backBallEnd = backBall;
                backBall = backBall.NextBall;
            }
            else
            {
                break ;
            }
        }

        if (matchingBalls.Count >= _ballChainConfig.MatchingCount - 1)
        {
            int count = matchingBalls.Count;
            
            PlayDestroyMatchingBalls(matchingBalls, matchingBalls.Count - 1);
            return true;
            
        }
        else
        {
            Debug.LogWarning("����ʧ�ܣ���ͬ��ɫ����������");
            return false;
        }
    }

    private void PlayDestroyMatchingBalls(List<Ball> matchingBalls, int count)
    {
        if (_frontBallEnd != null)
            _frontBallEnd.SetColor(Color.black);
        else
            Debug.LogError("_frontBallEnd == null");
        if (_backBallEnd != null)
            _backBallEnd.SetColor(Color.white);
        else
            Debug.LogError("_backBallEnd == null");
        _chainTracker.RemoveBallInMatch(matchingBalls, _backBallEnd, _frontBallEnd);
        foreach (var ball in matchingBalls)
        {
            ball.PlayDestroyAnimation(() =>
            {                         
                OnMatchBall?.Invoke(count);
                Debug.Log("��������");
                if (_ballChainConfig.zumaBoom != null) // ������������Prefab����
                {
                    GameObject effect = GameObject.Instantiate(
                        _ballChainConfig.zumaBoom,
                        ball.transform.position,
                        Quaternion.identity);
                }
                _ballProvider.ReturnBall(ball);
            });
        }
        
        _currentShootBall.ReturnBall();
        Debug.Log("�����ɹ�");
    }



}

