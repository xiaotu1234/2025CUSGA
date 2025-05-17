using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingletonMono<EnemyManager>
{
    public List<EnemyController> enemies;
    public int initialEnemyCount = 3;
    public List<GameObject> enemyPrefs = new List<GameObject>();
    public BoxCollider produceEnemyArea;
    public List<Color> colors = new List<Color>();

    [Tooltip("������ٸ�����û��Ѫ��֮����һ��һ����")]
    public int guaranteeCount = 5;
    [Tooltip("��ʼ����")]
    public float baseProbability = 0.1f; // ��ʼ����
    [Tooltip("ÿ��δ����ĸ�������")]
    public float probabilityIncrement = 0.15f; // ÿ��δ����ĸ�������
    private float _currentProbability;
    private int _missCount;

    

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
            int randomColor = Random.Range(0, colors.Count);
            GameObject enemyPrefab = enemyPrefs[randomIndex];
            if (enemyPrefab.TryGetComponent(out DashEnemyController controller))
            {
                controller.color = colors[randomColor];
            }
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
                enemy.GetFSM().OnDestroySelf();
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
            enemy.GetFSM().OnDestroySelf();
            Destroy(enemy.gameObject);
            enemies.Remove(enemy);
        }
    }
    // ��ȡ��������
    public int GetEnemyCount()
    {
        return enemies.Count;
    }

    public int GetMissCount()
    {
        return _missCount;
    }

    public void AddMissCount()
    {
         _missCount ++;
    }

    public void ResetMissCount()
    {
        _missCount=0;
    }

    public float GetCurrntProbability()
    {
        return _currentProbability;
    }

    public void AddCurrntProbability()
    {
        _currentProbability = Mathf.Clamp01(baseProbability + _missCount * probabilityIncrement);
    }

    public void ResetProbability()
    {
        _currentProbability = baseProbability;
    }

}
