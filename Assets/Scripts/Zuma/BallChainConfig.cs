using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;


[CreateAssetMenu(fileName = "BallChainConfig", menuName = "StaticData/BallChainConfg")]
public class BallChainConfig : ScriptableObject
{
    [Range(0.1f, 10)]
    public float ZumaBallRadius;
    [Tooltip("数值越大，球体之间的间距越小")]
    public float ZumaBallPositonOffset = 0.0f;
    [ReadOnly(true), Tooltip("不可修改")]
    public float SpacingBalls = 0f;
    [ReadOnly(true), Tooltip("不可修改")]
    public float DurationMovingOffset = 0.2f;
    public float MoveSpeed = 1f;
    public float MoveSpeedMultiplier = 1f;
    public float BoostDuration = 1f;
    [Tooltip("终点消除偏差，值越大，球体消除的越早")]
    public float EndOffset = 0.5f;
    [ReadOnly(true), Tooltip("不可修改")]
    public float DurationSpawnBall = 0.35f;
    [ReadOnly(true), Tooltip("不可修改")]
    public float CollisionOffset = 0.5f;
    [Tooltip("数值越大，射出球体的碰撞判定范围越大")]
    public float CollisionThreshold = 0.5f;
    [Tooltip("消除需要的最小数量")]
    public int MatchingCount = 3;


    private void OnValidate()
    {
        SpacingBalls = ZumaBallRadius * 2 - ZumaBallPositonOffset;
        CollisionThreshold = Mathf.Clamp(SpacingBalls + CollisionOffset, 0 ,SpacingBalls *2);
        ZumaBallPositonOffset = Mathf.Max(0f, ZumaBallPositonOffset);
        ZumaBallPositonOffset = Mathf.Min(ZumaBallPositonOffset, SpacingBalls);
        if (MoveSpeed > 0.01f)
        {
            DurationSpawnBall = SpacingBalls / MoveSpeed;
        }
    }

}
