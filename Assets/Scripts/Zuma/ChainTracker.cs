using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using System;

public class ChainTracker
{
    private BallChainConfig _ballChainConfig;
    public ChainTracker(BallChainConfig config)
    {
        _ballChainConfig = config;
        _ballSpace = config.SpacingBalls ;
    }
    // 核心数据结构
    private float _chainHeadDistance; // 链表头部累计移动距离
    private readonly LinkedList<Ball> _balls = new LinkedList<Ball>();
    private readonly Dictionary<Ball, float> _offsetFromHead = new Dictionary<Ball, float>(); // 球与头部的距离差

    public float ChainHeadDistance => _chainHeadDistance;
    public IEnumerable<Ball> Balls => _balls;
    private float _ballSpace; // 路径总长度



    public void AddChainHeadDistance(float delta)
    {
        _chainHeadDistance += delta;

        
    }

    // 添加球到链表尾部
    public void AddBallLast(Ball ball)
    {
        if (_balls.Count == 0)
        {
            _offsetFromHead[ball] = 0f;
        }
        else
        {
            // 新球的位置偏移 = 前一个球的偏移 + 间距
            float prevOffset = _offsetFromHead[_balls.Last.Value];
            _offsetFromHead[ball] = prevOffset + _ballSpace;
        }
        LinkedListNode<Ball> node =  _balls.AddLast(ball);
        if (node.Previous != null)
        {
            ball.PreviousBall = node.Previous.Value;
            node.Previous.Value.NextBall = ball;
        }
        

    }

    public void InsertBall(Ball newBall, Ball collisionBall)
    {
        LinkedListNode<Ball> collisionNode = _balls.Find(collisionBall);
     
        if (collisionNode != null)
        {
            Debug.LogWarning(_offsetFromHead[collisionNode.Value]);
            float offset = _offsetFromHead[collisionNode.Value];
            _offsetFromHead[newBall] = offset;
            LinkedListNode<Ball> previousNode = collisionNode;
            //collisionNode.Next.Value.SetColor(Color.black);
            //collisionNode.Value.SetColor(Color.white);
            _chainHeadDistance += _ballSpace;
            while (previousNode != null)
            {
                _offsetFromHead[previousNode.Value] += _ballSpace;
                previousNode = previousNode.Next;
            }
            
            if (collisionNode.Previous != null)
            {
                
                collisionNode.Previous.Value.NextBall = newBall;
                collisionNode.Value.PreviousBall =  newBall;
            }
            LinkedListNode<Ball> node = _balls.AddBefore(collisionNode, newBall);
            Ball prevBall = node.Previous.Value;
            Ball nextBall = node.Next.Value;
            prevBall.SetColor(Color.black);
            nextBall.SetColor(Color.white);
            newBall.PreviousBall = prevBall;
            newBall.NextBall = nextBall;
            Debug.LogWarning(_offsetFromHead[node.Value]);

        }
    }
    public void AddBallFirst(Ball ball)
    {
        if (_balls.Count == 0)
        {
            _offsetFromHead[ball] = 0f;
        }
        else
        { 
            _offsetFromHead[ball] = _ballSpace;
            LinkedListNode<Ball> firstNode = _balls.First;
        }
        _balls.AddFirst(ball);
    }


    // 移除指定球
    public void RemoveBall(Ball ball)
    {
        LinkedListNode<Ball> node = _balls.Find(ball);
        if (node == null) return;

        // 获取被移除球的偏移量
        float removedOffset = _offsetFromHead[ball];

        // 移除球并更新后续球的偏移量
        LinkedListNode<Ball> nextNode = node.Next;
        _chainHeadDistance -= removedOffset;
        while (nextNode != null)
        {
            _offsetFromHead[nextNode.Value] -= removedOffset;
            nextNode = nextNode.Next;
        }

        _offsetFromHead.Remove(ball);
        _balls.Remove(node);
    }

    // 获取某个球的目标距离（头部距离 - 该球的偏移）
    public float GetTargetDistanceForBall(Ball ball)
    {
        return _chainHeadDistance - _offsetFromHead[ball];
    }

    public void ClearUp()
    {
        _balls.Clear();
        _offsetFromHead.Clear();
        _chainHeadDistance = 0f;
    }

    public int GetCount()
    {
        return _balls.Count;
    }

    public float GetTotalChainLength()
    {
        return _balls.Count * _ballSpace;
    }
}
