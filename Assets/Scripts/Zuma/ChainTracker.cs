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
    // 核心数据结构
    private float _chainHeadDistance; // 链表头部累计移动距离
    private readonly LinkedList<Ball> _balls = new LinkedList<Ball>();
    private readonly Dictionary<Ball, float> _offsetFromHead = new Dictionary<Ball, float>(); // 球与头部的距离差

    public float ChainHeadDistance => _chainHeadDistance;
    public IEnumerable<Ball> Balls => _balls;
    private float _pathTotalLength; // 路径总长度


    public void Initialize(float pathLength)
    {
        _pathTotalLength = pathLength;
    }

    public void AddChainHeadDistance(float delta)
    {
        _chainHeadDistance += delta;

        //// 当链头超过路径总长度时，循环重置
        //if (_chainHeadDistance >= _pathTotalLength)
        //{
        //    _chainHeadDistance -= _ballChainConfig.SpacingBalls;
        //}
    }

    // 添加球到链表尾部
    public void AddBall(Ball ball)
    {
        if (_balls.Count == 0)
        {
            _offsetFromHead[ball] = 0f;
        }
        else
        {
            // 新球的位置偏移 = 前一个球的偏移 + 间距
            float prevOffset = _offsetFromHead[_balls.Last.Value];
            _offsetFromHead[ball] = prevOffset + _ballChainConfig.SpacingBalls;
        }
        _balls.AddLast(ball);
    }

    // 移除指定球
    public void RemoveBall(Ball ball)
    {
        Debug.LogWarning($"减去偏移量前的总长{_chainHeadDistance}");
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
        Debug.LogWarning($"减去偏移量后的总长{_chainHeadDistance}");
    }

    // 获取某个球的目标距离（头部距离 - 该球的偏移）
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
