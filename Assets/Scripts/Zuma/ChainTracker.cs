using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ChainTracker
{
    public ChainTracker(BallChainConfig config, float pathlength)
    {
        _ballChainConfig = config;
        _pathTotalLength = pathlength;
    }
    private BallChainConfig _ballChainConfig;
    // �������ݽṹ
    private float _chainHeadDistance; // ����ͷ���ۼ��ƶ�����
    private readonly LinkedList<Ball> _balls = new LinkedList<Ball>();
    private readonly Dictionary<Ball, float> _offsetFromHead = new Dictionary<Ball, float>(); // ����ͷ���ľ����

    public float ChainHeadDistance => _chainHeadDistance;
    public IEnumerable<Ball> Balls => _balls;
    private float _pathTotalLength; // ·���ܳ���


    public void Initialize(float pathLength)
    {
        _pathTotalLength = pathLength;
    }

    public void AddChainHeadDistance(float delta)
    {
        _chainHeadDistance += delta;

        //// ����ͷ����·���ܳ���ʱ��ѭ������
        //if (_chainHeadDistance >= _pathTotalLength)
        //{
        //    _chainHeadDistance -= _ballChainConfig.SpacingBalls;
        //}
    }

    // ���������β��
    public void AddBall(Ball ball)
    {
        if (_balls.Count == 0)
        {
            _offsetFromHead[ball] = 0f;
        }
        else
        {
            // �����λ��ƫ�� = ǰһ�����ƫ�� + ���
            float prevOffset = _offsetFromHead[_balls.Last.Value];
            _offsetFromHead[ball] = prevOffset + _ballChainConfig.SpacingBalls;
        }
        _balls.AddLast(ball);
    }

    // �Ƴ�ָ����
    public void RemoveBall(Ball ball)
    {
        Debug.LogWarning($"��ȥƫ����ǰ���ܳ�{_chainHeadDistance}");
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
        Debug.LogWarning($"��ȥƫ��������ܳ�{_chainHeadDistance}");
    }

    // ��ȡĳ�����Ŀ����루ͷ������ - �����ƫ�ƣ�
    public float GetTargetDistanceForBall(Ball ball)
    {
        return _chainHeadDistance - _offsetFromHead[ball];
    }

    

    public int GetCount()
    {
        return _balls.Count;
    }

    public float GetTotalChainLength()
    {
        return _balls.Count * _ballChainConfig.SpacingBalls;
    }
}
