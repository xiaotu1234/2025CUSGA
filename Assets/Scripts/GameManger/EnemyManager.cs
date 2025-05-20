using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : SingletonMono<EnemyManager>
{
    public List<EnemyController> enemies;
    public int initialEnemyCount = 3;
    [HideInInspector]public int nowCount = 1;
    public List<GameObject> enemyPrefs = new List<GameObject>();
    public BoxCollider produceEnemyArea;
    public List<Color> colors = new List<Color>();
    public Collider mapCollider;

    [Header("Ѫ������")]
    [Tooltip("������ٸ�����û��Ѫ��֮����һ��һ����")]
    public int guaranteeCount = 5;
    [Tooltip("��ʼ����")]
    public float baseProbability = 0.1f; // ��ʼ����
    [Tooltip("ÿ��δ����ĸ�������")]
    public float probabilityIncrement = 0.15f; // ÿ��δ����ĸ�������
    private float _currentProbability;
    private int _missCount;

    [Header("����")]
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
    private List<Color> _colors = new List<Color>();
    protected override void Awake()
    
    {
        base.Awake();
        foreach (var colorItem in colors)
        {
            _colors.Add(colorItem);
        }
        Debug.Log("_colors.Count = "+_colors.Count);

    }

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
        if (enemies.Count == 0 && BossManager.Instance.nowStage == 1 && !isRetry && nowCount > initialEnemyCount)
        {
            BossManager.Instance.nowStage = 2;
        }
        if (BossManager.Instance.nowStage == 1 && produceTimer >= produceCooldown && nowCount<=initialEnemyCount)
        {
            produceTimer = 0;
            ProduceEnemyOnce(0.5f);
            nowCount++;
        }
        //���л�����һ֡����
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

    public void ResetColorList()
    {
        Debug.Log("ResetColorList");
        Debug.Log("_colors.Count = " + _colors.Count);
        foreach (var colorItem in _colors)
        {
            colors.Add(colorItem);
        }
    }

    public void ProduceEnemy()
    {



        if(colors.Count == 0)
            ResetColorList();

        for (int i = 0; i < 1; i++)
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

            Vector3 spawnPosition = Vector3.zero;
            bool validPositionFound = false;

            for (int attempt = 0; attempt < 10; attempt++)
            {
                // ���������������λ��
                spawnPosition = new Vector3(
                    Random.Range(bounds.min.x, bounds.max.x),
                    1,
                    Random.Range(bounds.min.z, bounds.max.z)
                );

                // �������ҵľ���
                if (Vector3.Distance(spawnPosition, PlayerManager.Instance.player.transform.position) >= minDistanceFromPlayer)
                {
                    validPositionFound = true;
                    break;
                }
            }

            // ����Ҳ������ʵ�λ�ã�ʹ����������
            if (!validPositionFound)
            {
                spawnPosition = bounds.center;
                spawnPosition.y = 1;
                Debug.LogWarning($"Failed to find valid spawn position for enemy {i}. Using center instead.");
            }

            // ʵ��������
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }
    public void ProduceEnemyOnce(float probability)
    {
        if (enemies.Count > maxEnemyCount)
            return;
        // ����Ƿ��п��õĵ���Ԥ����
        if (enemyPrefs == null || enemyPrefs.Count == 0)
        {
            Debug.LogWarning("No enemy prefabs assigned!");
            return;
        }
        GameObject enemyPrefab;
        // ���ݸ���ѡ��Ԥ���壨����ǰ����Ԥ����ΪĿ�����ͣ�
        if (Random.value <= probability)
        {
            enemyPrefab = enemyPrefs[0]; // ��һ�ֵ��ˣ����ʸߣ�
        }
        else
        {
            int r = Random.Range(1, enemyPrefs.Count - 1);
            enemyPrefab = enemyPrefs[r]; // �������ˣ����ʵͣ�
        }
        int randomColor = Random.Range(0, colors.Count);
        if (enemyPrefab.TryGetComponent(out DashEnemyController controller))
        {
            controller.color = colors[randomColor];
        }
        // ��ȡ��������ı߽�
        Bounds bounds = produceEnemyArea.bounds;

        Vector3 spawnPosition = Vector3.zero;
        bool validPositionFound = false;

        for (int attempt = 0; attempt < 10; attempt++)
        {
            // ���������������λ��
            spawnPosition = new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                1,
                Random.Range(bounds.min.z, bounds.max.z)
            );

            // �������ҵľ���
            if (Vector3.Distance(spawnPosition, PlayerManager.Instance.player.transform.position) >= minDistanceFromPlayer)
            {
                validPositionFound = true;
                break;
            }
        }

        // ����Ҳ������ʵ�λ�ã�ʹ����������
        if (!validPositionFound)
        {
            spawnPosition = bounds.center;
            spawnPosition.y = 1;
        }

        // ʵ��������
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
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
                // ��������������һЩ���ٵ��˵Ķ����߼������粥������������
                enemy.GetFSM().OnDestroySelf(false);
                Destroy(enemy.gameObject);
            }
        }
        enemies.Clear();
        //ProduceEnemy();
        produceTimer = produceCooldown;
    }

    // ������ʵ�������ض�����
    public void DestroyEnemy(EnemyController enemy)
    {
        if (enemy != null && enemies.Contains(enemy))
        {
            enemy.GetFSM().OnDestroySelf(true);
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
