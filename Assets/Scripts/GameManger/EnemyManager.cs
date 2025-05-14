using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingletonMono<EnemyManager>
{
    public List<EnemyController> enemies;
    public int initialEnemyCount = 3;
    public List<GameObject> enemyPrefs = new List<GameObject>();
    public BoxCollider produceEnemyArea;

    public void ProduceEnemy()
    {
        for (int i = 0; i < initialEnemyCount; i++)
        {
            // ����Ƿ��п��õĵ���Ԥ����
            if (enemyPrefs == null || enemyPrefs.Count == 0)
            {
                Debug.LogWarning("No enemy prefabs assigned!");
                return;
            }
            // ���ѡ��һ������Ԥ����
            int randomIndex = Random.Range(0, enemyPrefs.Count);
            GameObject enemyPrefab = enemyPrefs[randomIndex];
            // ��ȡ��������ı߽�
            Bounds bounds = produceEnemyArea.bounds;

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

    // ע���µ��˵�������
    public void RegisterEnemy(EnemyController enemy)
    {
        if (enemy != null && !enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    // �������е���
    public void DestroyAllEnemies()
    {
        foreach (EnemyController enemy in enemies)
        {
            if (enemy != null)
            {
                // �������������һЩ���ٵ��˵Ķ����߼������粥������������
                Destroy(enemy.gameObject);
            }
        }
        enemies.Clear();
    }

    // ������ʵ�������ض�����
    public void DestroyEnemy(EnemyController enemy)
    {
        if (enemy != null && enemies.Contains(enemy))
        {
            Destroy(enemy.gameObject);
            enemies.Remove(enemy);
        }
    }
    // ��ȡ��������
    public int GetEnemyCount()
    {
        return enemies.Count;
    }
}
