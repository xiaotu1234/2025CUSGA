using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss_1_Shoot", menuName = "ScriptableObject/Boss_1/Boss_1_Shoot", order = 0)]
public class Boss_1_Shoot : EnemyState
{
    Boss_1_Controller boss;
    [Header("Shoot Setting")]
    public float shootContinueTime;
    private float shootTimer;

    public override void OnAwake()
    {

    }

    public override void OnEnter()
    {
        shootTimer = 0;
        boss = BossManager.Instance.boss_1;
        StartShoot();
    }

    public override void OnExit()
    {
        StopShoot();
    }

    public override void OnUpdate()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer > shootContinueTime)
            boss.stateMachine.TransitionState("Boss_1_Attack");
    }
    private void StartShoot()
    {
        foreach (GameObject cube in boss.shootCube)
        {
            cube.SetActive(true);
        }
    }
    private void StopShoot()
    {
        foreach (GameObject cube in boss.shootCube)
        {
            cube.SetActive(false);
        }
    }

}
