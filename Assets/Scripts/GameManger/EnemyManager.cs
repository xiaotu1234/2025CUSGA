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
            // 检查是否有可用的敌人预制体
            if (enemyPrefs == null || enemyPrefs.Count == 0)
            {
                Debug.LogWarning("No enemy prefabs assigned!");
                return;
            }
            // 随机选择一个敌人预制体
            int randomIndex = Random.Range(0, enemyPrefs.Count);
            GameObject enemyPrefab = enemyPrefs[randomIndex];
            // 获取生成区域的边界
            Bounds bounds = produceEnemyArea.bounds;

            // 在区域内随机生成位置
            Vector3 randomPosition = new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                1,
                Random.Range(bounds.min.z, bounds.max.z)
            );

            // 实例化敌人
            Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
        }
    }

    // 注册新敌人到管理器
    public void RegisterEnemy(EnemyController enemy)
    {
        if (enemy != null && !enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    // 销毁所有敌人
    public void DestroyAllEnemies()
    {
        foreach (EnemyController enemy in enemies)
        {
            if (enemy != null)
            {
                // 可以在这里添加一些销毁敌人的额外逻辑，比如播放死亡动画等
                Destroy(enemy.gameObject);
            }
        }
        enemies.Clear();
    }

    // 按敌人实例销毁特定敌人
    public void DestroyEnemy(EnemyController enemy)
    {
        if (enemy != null && enemies.Contains(enemy))
        {
            Destroy(enemy.gameObject);
            enemies.Remove(enemy);
        }
    }
    // 获取敌人数量
    public int GetEnemyCount()
    {
        return enemies.Count;
    }
}
