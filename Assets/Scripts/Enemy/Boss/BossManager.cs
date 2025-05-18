using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : SingletonMono<BossManager>
{
    #region 事件
    public event Action OnEnterPhase2;
    public event Action OnEnterPhase3;
    public event Action<float> OnTakeDamageByZuma;
    public event Action OnBossDie;
    #endregion
    public Boss_1_Controller boss_1;
    [Header("Boss血量")]
    public float healthInPhase3 = 100;
    [Header("消除一个球扣多少血")]
    public float damageRate = 1;
    private float _currentHealth;
    public GameObject path;
    public GameObject zumaManager;
    [Header("预制体")]
    public GameObject boss_1_Prefab;
    public GameObject zumaManager_Prefab;
    public GameObject path_Prefab;
    public GameObject BossUI;

    private Vector3 boss_1_Position;
    private Vector3 zumaManager_Position;
    private Vector3 path_Position;
    [HideInInspector] public int nowStage=1;
    private int lastStage = 1;

    //策划加的动画变量
    public GameObject boss1anim;

    // Start is called before the first frame update
    void Start()
    {
        boss_1_Position= boss_1.transform.position;
        zumaManager_Position = zumaManager.transform.position;
        path_Position = path.transform.position;
        boss_1.gameObject.SetActive(false);
        zumaManager.SetActive(false);
        path.SetActive(false);
        _currentHealth = healthInPhase3;
    }
    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        AttachingBallChainHandler.OnMatchBall -= TakeDamageByZuma;
    }
    // Update is called once per frame
    void Update()
    {
        if (lastStage != nowStage && nowStage == 2) 
        {
            boss1anim.SetActive(true);
            StartCoroutine(Boss2aterDelay(2f));
            lastStage = nowStage;
        }
        if (lastStage != nowStage && nowStage == 3)
        {
            boss1anim.SetActive(true);
            ActiveBoss2();
            lastStage = nowStage;
        }
        if (nowStage == 3)
        {
            if (_currentHealth <= 0)
            {
                OnBossDie?.Invoke();
            }
        }

    }
    private void ActiveBoss1()
    {

        OnEnterPhase2?.Invoke();
        BossUI.SetActive(true);
        boss_1.gameObject.SetActive(true);
    }

    
    private void ActiveBoss2()
    {
        
        //这里是一阶段死亡后生成二阶段的逻辑，想加动画的话在这里加
        OnEnterPhase3?.Invoke();
        path.SetActive(true);
        zumaManager.SetActive(true);
        AttachingBallChainHandler.OnMatchBall += TakeDamageByZuma;

    }
    IEnumerator Boss2aterDelay(float delay)
    {
        // 等待 delay 秒
        yield return new WaitForSecondsRealtime(delay); // 使用 Realtime 不受 Time.timeScale 影响
        ActiveBoss1();
        boss1anim.SetActive(false);
    }

    public void TakeDamageByZuma(int count)
    {
        
        float damage = damageRate * count;
        OnTakeDamageByZuma?.Invoke(damage);
    }


    public void ResetBoss()
    {

        BossUI?.SetActive(false);

        if(boss_1.gameObject)
            Destroy(boss_1.gameObject);

        boss_1 = Instantiate(boss_1_Prefab).GetComponent<Boss_1_Controller>();
        //这里需要把二阶段boss的血量调到满血
        //todo

        boss_1.gameObject.SetActive(false);
        zumaManager.SetActive(false);
        path.SetActive(false);

        boss_1.gameObject.transform.position = boss_1_Position;
        nowStage = 1;
        lastStage = nowStage;

    }
}

