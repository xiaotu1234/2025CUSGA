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
    private float minDistanceFromPlayer;
    public override void OnAwake()
    {
    }

    public override void OnEnter()
    {
        boss = BossManager.Instance.boss_1;
        produceTimer = 0;
        produceCDTimer = 0;
        minDistanceFromPlayer = EnemyManager.Instance.minDistanceFromPlayer;
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
                //�л�Ϊ��״̬
                boss.stateMachine.TransitionState("Boss_1_Rage");
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
        //// ����Ƿ��п��õĵ���Ԥ����
        //if (boss.enemyPrefs == null || boss.enemyPrefs.Count == 0)
        //{
        //    Debug.LogWarning("No enemy prefabs assigned!");
        //    return;
        //}
        //// ���ѡ��һ������Ԥ����
        //int randomIndex = Random.Range(0, boss.enemyPrefs.Count);
        //GameObject enemyPrefab = boss.enemyPrefs[randomIndex];
        //// ��ȡ��������ı߽�
        //Bounds bounds = boss.produceEnemyArea.bounds;

        //Vector3 spawnPosition = Vector3.zero;
        //bool validPositionFound = false;

        //for (int attempt = 0; attempt < 10; attempt++)
        //{
        //    // ���������������λ��
        //    spawnPosition = new Vector3(
        //        Random.Range(bounds.min.x, bounds.max.x),
        //        1,
        //        Random.Range(bounds.min.z, bounds.max.z)
        //    );

        //    // �������ҵľ���
        //    if (Vector3.Distance(spawnPosition, PlayerManager.Instance.player.transform.position) >= minDistanceFromPlayer)
        //    {
        //        validPositionFound = true;
        //        break;
        //    }
        //}

        //// ����Ҳ������ʵ�λ�ã�ʹ����������
        //if (!validPositionFound)
        //{
        //    spawnPosition = bounds.center;
        //    spawnPosition.y = 1;
        //}

        //// ʵ��������
        //Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        EnemyManager.Instance.ProduceEnemyOnce(0.33f);
    }
}
