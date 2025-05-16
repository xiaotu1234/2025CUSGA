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
    // �������ݽṹ
    private float _chainHeadDistance; // ����ͷ���ۼ��ƶ�����
    private readonly LinkedList<Ball> _balls = new LinkedList<Ball>();
    private readonly Dictionary<Ball, float> _offsetFromHead = new Dictionary<Ball, float>(); // ����ͷ���ľ����

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

    // ���������β��
    public void AddBallLast(Ball ball)
    {
        if (_balls.Count == 0)
        {
            _offsetFromHead[ball] = 0f;
        }
        else
        {
            // �����λ��ƫ�� = ǰһ�����ƫ�� + ���
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
    // �Ƴ�ָ����
    public void RemoveBall(Ball ball)
    {
        LinkedListNode<Ball> node = _balls.Find(ball);
        if (node == null) return;

        // ��ȡ���Ƴ����ƫ����
        float removedOffset = _offsetFromHead[ball];

        // �Ƴ��򲢸��º������ƫ����
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

    // ��ȡĳ�����Ŀ����루ͷ������ - �����ƫ�ƣ�
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
