using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using System;

public class ChainTracker
{
    public ChainTracker(BallChainConfig config)
    {
        _ballChainConfig = config;
        _ballSpace = config.SpacingBalls ;
    }
    private BallChainConfig _ballChainConfig;
    // 核心数据结构
    private float _chainHeadDistance; // 链表头部累计移动距离
    private readonly LinkedList<Ball> _balls = new LinkedList<Ball>();
    private readonly Dictionary<Ball, float> _offsetFromHead = new Dictionary<Ball, float>(); // 球与头部的距离差

    public float ChainHeadDistance => _chainHeadDistance;
    public IEnumerable<Ball> Balls => _balls;
    private float _ballSpace; 

    public void ReduceChainHeadDistance(float delta)
    {
        _chainHeadDistance -= delta;


    }


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
        LinkedListNode<Ball> existingNode = _balls.Find(collisionBall);
        if (existingNode != null)
        {
            LinkedListNode<Ball> nextNode = existingNode;
            while (nextNode != null)
            {
                _offsetFromHead[nextNode.Value] += _ballSpace;
                nextNode = nextNode.Next;
            }
            LinkedListNode<Ball> node = _balls.AddAfter(existingNode, newBall);
            if (node.Previous != null)
            {
                newBall.PreviousBall = node.Previous.Value;
                node.Previous.Value.NextBall = newBall;
            }
            float prevOffset = _offsetFromHead[node.Previous.Value];
            _offsetFromHead[newBall] = prevOffset + _ballSpace;

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

    public void RemoveBallInMatch(List<Ball> matchList, Ball endOfList, Ball firstOfList)
    {

        LinkedListNode<Ball> endNode = _balls.Find(endOfList);
        float endOffset = _offsetFromHead[endOfList];
        float firstOffset = _offsetFromHead[firstOfList];

        float delta = endOffset - firstOffset + _ballSpace;

        LinkedListNode<Ball> nextNode = endNode.Next;
        while (nextNode != null)
        {
            _offsetFromHead[nextNode.Value] -= delta;
            nextNode = nextNode.Next;
        }
        foreach (Ball ball in matchList)
        {
            _offsetFromHead.Remove(ball);
            LinkedListNode<Ball> node = _balls.Find(ball);
            _balls.Remove(node);
        };
        _chainHeadDistance -= delta; 

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
