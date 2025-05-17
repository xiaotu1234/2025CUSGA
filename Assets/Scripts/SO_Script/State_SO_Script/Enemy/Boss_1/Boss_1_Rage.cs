using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss_1_Rage", menuName = "ScriptableObject/Boss_1/Boss_1_Rage", order = 0)]
public class Boss_1_Rage : EnemyState
{
    Boss_1_Controller boss;
    [Header("Attack Setting")]
    //需要大于触手攻击动作时间
    public float attackCooldown;
    private float attackTimer;


    public override void OnAwake()
    {
    }

    public override void OnEnter()
    {
        attackTimer = 0;
        boss = BossManager.Instance.boss_1;
        StartShoot();
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        if (attackTimer > attackCooldown)
        {
            // 随机选择触手进行攻击
            List<Tentacle> selectedTentacles = GetRandomTentacles(boss.attackTentacleCount);
            // 触发所有选中触手的攻击
            foreach (Tentacle tentacle in selectedTentacles)
            {
                if (tentacle != null)
                {
                    tentacle.Attack();
                }
            }
            attackTimer = 0;
        }
        attackTimer += Time.deltaTime;
        
    }
    private void StartShoot()
    {
        foreach (GameObject cube in boss.shootCube)
        {
            cube.SetActive(true);
        }
    }
    private List<Tentacle> GetRandomTentacles(int count)
    {
        List<Tentacle> result = new List<Tentacle>();

        // 如果请求数量大于列表数量，返回全部
        if (count >= boss.tentacles.Count)
        {
            return new List<Tentacle>(boss.tentacles);
        }

        // 创建临时列表用于随机选择
        List<Tentacle> tempList = new List<Tentacle>(boss.tentacles);

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, tempList.Count);
            result.Add(tempList[randomIndex]);
            tempList.RemoveAt(randomIndex); // 避免重复选择
        }

        return result;
    }

}
