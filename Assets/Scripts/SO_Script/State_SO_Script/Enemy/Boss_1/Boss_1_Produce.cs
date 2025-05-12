using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss_1_Produce", menuName = "ScriptableObject/Boss_1/Boss_1_Produce", order = 0)]
public class Boss_1_Produce : EnemyState
{
    Boss_1_Controller boss;
    [Header("Produce enemy Setting")]
    public float produceContinueTime;
    public float produceCooldown;
    private float produceTimer;
    private float produceCDTimer;

    public override void OnAwake()
    {
    }

    public override void OnEnter()
    {
        boss = BossManager.Instance.boss_1;
        produceTimer = 0;
        produceCDTimer = 0;
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        produceTimer += Time.deltaTime;
        produceCDTimer += Time.deltaTime;
        if (produceTimer > produceContinueTime)
        {
            if (boss.tentacles.Count <= 3)
                Debug.Log(boss.tentacles.Count);
            //�л�Ϊ��״̬
            //boss.stateMachine.TransitionState("");
            else
                //�л�Ϊ���״̬
                boss.stateMachine.TransitionState("Boss_1_Shoot");
        }
        if (produceCDTimer > produceCooldown)
        {
            ProduceEnemy();
            produceCDTimer = 0;
        }

    }
    private void ProduceEnemy()
    {
        // ����Ƿ��п��õĵ���Ԥ����
        if (boss.enemyPrefs == null || boss.enemyPrefs.Count == 0)
        {
            Debug.LogWarning("No enemy prefabs assigned!");
            return;
        }
        // ���ѡ��һ������Ԥ����
        int randomIndex = Random.Range(0, boss.enemyPrefs.Count);
        GameObject enemyPrefab = boss.enemyPrefs[randomIndex];
        // ��ȡ��������ı߽�
        Bounds bounds = boss.produceEnemyArea.bounds;

        // ���������������λ��
        Vector3 randomPosition = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            1,
            Random.Range(bounds.min.z, bounds.max.z)
        );

        // ʵ��������
        Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
    }
}
