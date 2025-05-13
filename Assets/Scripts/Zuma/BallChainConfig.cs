using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;


[CreateAssetMenu(fileName = "BallChainConfig", menuName = "StaticData/BallChainConfg")]
public class BallChainConfig : ScriptableObject
{
    [Range(0.1f, 10)]
    public float ZumaBallRadius;
    [Tooltip("��ֵԽ������֮��ļ��ԽС")]
    public float ZumaBallPositonOffset = 0.0f;
    [ReadOnly(true), Tooltip("�����޸�")]
    public float SpacingBalls = 0f;
    [ReadOnly(true), Tooltip("�����޸�")]
    public float DurationMovingOffset = 0.2f;
    public float MoveSpeed = 1f;
    public float MoveSpeedMultiplier = 1f;
    public float BoostDuration = 1f;
    [Tooltip("�յ�����ƫ�ֵԽ������������Խ��")]
    public float EndOffset = 0.5f;
    [ReadOnly(true), Tooltip("�����޸�")]
    public float DurationSpawnBall = 0.35f;
    [ReadOnly(true), Tooltip("�����޸�")]
    public float CollisionOffset = 0.5f;
    [Tooltip("��ֵԽ������������ײ�ж���ΧԽ��")]
    public float CollisionThreshold = 0.5f;
    [Tooltip("������Ҫ����С����")]
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
