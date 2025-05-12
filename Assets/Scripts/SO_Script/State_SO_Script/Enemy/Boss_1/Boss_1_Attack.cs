using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss_1_Attack", menuName = "ScriptableObject/Boss_1/Boss_1_Attack", order = 0)]
public class Boss_1_Attack : EnemyState
{
    Boss_1_Controller boss;
    [Header("Attack Setting")]
    //��Ҫ���ڴ��ֹ�������ʱ��
    public float attackContinueTime;
    private float attackTimer;

    public override void OnAwake()
    {
    }

    public override void OnEnter()
    {
        boss = BossManager.Instance.boss_1;
        attackTimer = 0;
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
        boss.attackTentacleCount += 1;
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {
        attackTimer += Time.deltaTime;
        if (boss.tentacles.Count <= 6&&boss.isFirstStage)
        {
            boss.isFirstStage = false;
            boss.stateMachine.TransitionState("Boss_1_Produce");
        }
        if (attackTimer > attackContinueTime)
            boss.stateMachine.TransitionState("Boss_1_Shoot");
        if (boss.tentacles.Count <= 3)
            boss.stateMachine.TransitionState("Boss_1_Rage");
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
