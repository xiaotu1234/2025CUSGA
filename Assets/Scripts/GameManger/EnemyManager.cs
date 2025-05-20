using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : SingletonMono<EnemyManager>
{
    public List<EnemyController> enemies;
    public int initialEnemyCount = 3;
    public List<GameObject> enemyPrefs = new List<GameObject>();
    public BoxCollider produceEnemyArea;
    public List<Color> colors = new List<Color>();
    public Collider mapCollider;

    [Tooltip("如果多少个怪物没掉血包之后下一个一定掉")]
    public int guaranteeCount = 5;
    [Tooltip("初始概率")]
    public float baseProbability = 0.1f; // 初始概率
    [Tooltip("每次未掉落的概率增量")]
    public float probabilityIncrement = 0.15f; // 每次未掉落的概率增量
    private float _currentProbability;
    private int _missCount;

    public float firstMonsterProbability;
    public float produceCooldown;
    public float thirdStageProduceCooldown;
    public float minDistanceFromPlayer;
    public int maxEnemyCount=8;
    private float produceTimer;

    [SerializeField] private Button startButton;
    [HideInInspector] public bool isStarted=false;
    private bool isHided = false;
    [HideInInspector] public bool isRetry = false;

    private void Start()
    {
        ProduceEnemy();
        if (startButton != null)
        {
            startButton.onClick.AddListener(ShowAllEnemies);
        }
    }
    private void HideAllEnemies()
    {
        foreach (EnemyController enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.gameObject.SetActive(false);
            }
        }
    }
    public void ShowAllEnemies()
    {
        isStarted = true;
        foreach (EnemyController enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.gameObject.SetActive(true);
            }
        }
    }
    private void Update()
    {
        if(!isHided)
        {
            HideAllEnemies();
            isHided = true;
        }
        if (enemies.Count == 0&&BossManager.Instance.nowStage == 1 && !isRetry)
        {
            BossManager.Instance.nowStage = 2;
        }
        //让切换在下一帧进行
        if (isRetry)
            isRetry = false;
        if (BossManager.Instance.nowStage == 2 && produceTimer >= produceCooldown)
        {
            produceTimer = 0;
            ProduceEnemyOnce(0.5f);

        }
        if (BossManager.Instance.nowStage == 3 && produceTimer >= thirdStageProduceCooldown)
        {
            produceTimer = 0;
            ProduceEnemyOnce(firstMonsterProbability);
        }

        produceTimer += Time.deltaTime;
        
    }


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
            int randomColor = Random.Range(0, colors.Count);
            GameObject enemyPrefab = enemyPrefs[randomIndex];
            if (enemyPrefab.TryGetComponent(out DashEnemyController controller))
            {
                controller.color = colors[randomColor];
            }
            // 获取生成区域的边界
            Bounds bounds = produceEnemyArea.bounds;

            Vector3 spawnPosition = Vector3.zero;
            bool validPositionFound = false;

            for (int attempt = 0; attempt < 10; attempt++)
            {
                // 在区域内随机生成位置
                spawnPosition = new Vector3(
                    Random.Range(bounds.min.x, bounds.max.x),
                    1,
                    Random.Range(bounds.min.z, bounds.max.z)
                );

                // 检查与玩家的距离
                if (Vector3.Distance(spawnPosition, PlayerManager.Instance.player.transform.position) >= minDistanceFromPlayer)
                {
                    validPositionFound = true;
                    break;
                }
            }

            // 如果找不到合适的位置，使用区域中心
            if (!validPositionFound)
            {
                spawnPosition = bounds.center;
                spawnPosition.y = 1;
                Debug.LogWarning($"Failed to find valid spawn position for enemy {i}. Using center instead.");
            }

            // 实例化敌人
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }
    public void ProduceEnemyOnce(float probability)
    {
        if (enemies.Count > maxEnemyCount)
            return;
        // 检查是否有可用的敌人预制体
        if (enemyPrefs == null || enemyPrefs.Count == 0)
        {
            Debug.LogWarning("No enemy prefabs assigned!");
            return;
        }
        GameObject enemyPrefab;
        // 根据概率选择预制体（假设前两个预制体为目标类型）
        if (Random.value <= probability)
        {
            enemyPrefab = enemyPrefs[0]; // 第一种敌人（概率高）
        }
        else
        {
            int r = Random.Range(1, enemyPrefs.Count - 1);
            enemyPrefab = enemyPrefs[r]; // 其他敌人（概率低）
        }
        int randomColor = Random.Range(0, colors.Count);
        if (enemyPrefab.TryGetComponent(out DashEnemyController controller))
        {
            controller.color = colors[randomColor];
        }
        // 获取生成区域的边界
        Bounds bounds = produceEnemyArea.bounds;

        Vector3 spawnPosition = Vector3.zero;
        bool validPositionFound = false;

        for (int attempt = 0; attempt < 10; attempt++)
        {
            // 在区域内随机生成位置
            spawnPosition = new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                1,
                Random.Range(bounds.min.z, bounds.max.z)
            );

            // 检查与玩家的距离
            if (Vector3.Distance(spawnPosition, PlayerManager.Instance.player.transform.position) >= minDistanceFromPlayer)
            {
                validPositionFound = true;
                break;
            }
        }

        // 如果找不到合适的位置，使用区域中心
        if (!validPositionFound)
        {
            spawnPosition = bounds.center;
            spawnPosition.y = 1;
        }

        // 实例化敌人
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
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
                enemy.GetFSM().OnDestroySelf(false);
                Destroy(enemy.gameObject);
            }
        }
        enemies.Clear();
        //ProduceEnemy();
        produceTimer = produceCooldown;
    }

    // 按敌人实例销毁特定敌人
    public void DestroyEnemy(EnemyController enemy)
    {
        if (enemy != null && enemies.Contains(enemy))
        {
            enemy.GetFSM().OnDestroySelf(true);
            Destroy(enemy.gameObject);
            enemies.Remove(enemy);
        }
    }
    // 获取敌人数量
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
