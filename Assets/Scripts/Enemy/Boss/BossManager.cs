using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BossManager : SingletonMono<BossManager>
{
    #region �¼�
    public event Action OnEnterPhase2;
    public event Action OnEnterPhase3;
    public event Action<float> OnTakeDamageByZuma;
    public event Action OnBossDie;
    public event Action OnResetBoss;
    #endregion
    public Boss_1_Controller boss_1;
    [Header("BossѪ��")]
    public float healthInPhase3 = 100;
    [Header("����һ����۶���Ѫ")]
    public float damageRate = 1;
    private float _currentHealth;
    public GameObject path;
    public GameObject zumaManager;
    [Header("Ԥ����")]
    public GameObject boss_1_Prefab;
    public GameObject zumaManager_Prefab;
    public GameObject path_Prefab;
    public GameObject BossUI;
    public GameObject chushouGroup;

    private Vector3 boss_1_Position;
    private Vector3 zumaManager_Position;
    private Vector3 path_Position;
    [HideInInspector] public int nowStage=1;
    private int lastStage = 1;

    //�߻��ӵĶ�������
    public GameObject boss1anim;
    public GameObject endUI;
    public GameObject boss2anim;
    public Animator mapAnmi;
    public Animator maincamera;

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
            AudioManager.Instance.PlayBGM(2);
            boss1anim.SetActive(true);
            StartCoroutine(Boss2aterDelay(2f));
            lastStage = nowStage;
        }
        if (lastStage != nowStage && nowStage == 3)
        {
            mapAnmi.SetBool("change3",true);
            boss2anim.SetActive(true);
            ActiveBoss2();
            lastStage = nowStage;
        }
        if (nowStage == 3)
        {
            if (_currentHealth <= 0)
            {
                maincamera.SetTrigger("end");
                boss2anim.GetComponent<Animator>().SetTrigger("end");
                OnBossDie?.Invoke();
                EnemyManager.Instance.isOver = true;
                StartCoroutine(enduiAfterDelay(3.1f));

            }
        }

    }
    IEnumerator enduiAfterDelay(float delay)
    {
        // �ȴ� delay ��
        yield return new WaitForSecondsRealtime(delay);
        endUI.SetActive(true);
        
    }
    private void ActiveBoss1()
    {

        OnEnterPhase2?.Invoke();
        BossUI.SetActive(true);
        boss_1.gameObject.SetActive(true);
    }

    
    private void ActiveBoss2()
    {
        
        //������һ�׶����������ɶ��׶ε��߼�����Ӷ����Ļ��������
        OnEnterPhase3?.Invoke();
        path.SetActive(true);
        zumaManager.SetActive(true);
        if(chushouGroup != null)
            chushouGroup.SetActive(true);
        else
            Debug.LogError("chushouGroupδ����");
        AttachingBallChainHandler.OnMatchBall += TakeDamageByZuma;

    }
    IEnumerator Boss2aterDelay(float delay)
    {
        // �ȴ� delay ��
        yield return new WaitForSecondsRealtime(delay); // ʹ�� Realtime ���� Time.timeScale Ӱ��
        ActiveBoss1();
        boss1anim.SetActive(false);
    }

    public void TakeDamageByZuma(int count)
    {
        
        float damage = damageRate * count;
        _currentHealth -= damage;
        OnTakeDamageByZuma?.Invoke(damage);
    }


    public void ResetBoss()
    {

        BossUI?.SetActive(false);

        if (boss_1 != null)
            Destroy(boss_1.gameObject);

        boss_1 = Instantiate(boss_1_Prefab).GetComponent<Boss_1_Controller>();
        OnResetBoss?.Invoke();
        _currentHealth = healthInPhase3;
        //������Ҫ�Ѷ��׶�boss��Ѫ��������Ѫ
        //todo

        boss_1.gameObject.SetActive(false);
        zumaManager.SetActive(false);
        path.SetActive(false);
        EnemyManager.Instance.ProduceEnemy();

        boss_1.gameObject.transform.position = boss_1_Position;
        EnemyManager.Instance.nowCount = 1;
        nowStage = 1;
        lastStage = nowStage;
    }

    public void EndPhase3()
    {
        if (nowStage != 3)
        {
            Debug.LogWarning($"��ǰ�׶Σ�{nowStage}");
        }else
        {
            int count = (int)Math.Ceiling(healthInPhase3 / damageRate);
            TakeDamageByZuma(count);
        }
    }
    public void EndPhase2()
    {
        if (nowStage != 2)
        {
            Debug.LogWarning($"��ǰ�׶Σ�{nowStage}");
        }
        else
        {
            boss_1.CleanTentacle();
        }
    }
}

