using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;


[CreateAssetMenu(fileName = "BallChainConfig", menuName = "StaticData/BallChainConfg")]
public class BallChainConfig : ScriptableObject
{
    [Range(0.1f, 10)]
    public float ZumaBallRadius;
    public float ZumaBallPositonOffset = 0.0f;
    [ReadOnly(true)]
    public float SpacingBalls = 0f;
    public float DurationMovingOffset = 0.2f;
    public float MoveSpeed = 1f;
    public float MoveSpeedMultiplier = 1f;
    public float BoostDuration = 1f;
    public float DurationSpawnBall = 0.35f;

    private void OnValidate()
    {
        SpacingBalls = ZumaBallRadius * 2 - ZumaBallPositonOffset;
        ZumaBallPositonOffset = Mathf.Max(0f, ZumaBallPositonOffset);
        ZumaBallPositonOffset = Mathf.Min(ZumaBallPositonOffset, SpacingBalls);
        if (MoveSpeed > 0.01f)
        {
            DurationSpawnBall = SpacingBalls / MoveSpeed;
        }
    }

}
