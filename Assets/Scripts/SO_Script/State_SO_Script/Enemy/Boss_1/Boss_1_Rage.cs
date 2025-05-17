using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss_1_Rage", menuName = "ScriptableObject/Boss_1/Boss_1_Rage", order = 0)]
public class Boss_1_Rage : EnemyState
{
    Boss_1_Controller boss;
    [Header("Attack Setting")]
    //��Ҫ���ڴ��ֹ�������ʱ��
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
            // ���ѡ���ֽ��й���
            List<Tentacle> selectedTentacles = GetRandomTentacles(boss.attackTentacleCount);
            // ��������ѡ�д��ֵĹ���
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

        // ����������������б�����������ȫ��
        if (count >= boss.tentacles.Count)
        {
            return new List<Tentacle>(boss.tentacles);
        }

        // ������ʱ�б��������ѡ��
        List<Tentacle> tempList = new List<Tentacle>(boss.tentacles);

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, tempList.Count);
            result.Add(tempList[randomIndex]);
            tempList.RemoveAt(randomIndex); // �����ظ�ѡ��
        }

        return result;
    }

}
