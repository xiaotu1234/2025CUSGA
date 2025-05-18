using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : SingletonMono<BossManager>
{
    #region 事件
    public event Action OnEnterPhase2;
    #endregion
    public Boss_1_Controller boss_1;

    public GameObject path;
    public GameObject zumaManager;

    public GameObject boss_1_Prefab;
    public GameObject zumaManager_Prefab;
    public GameObject path_Prefab;

    private Vector3 boss_1_Position;
    private Vector3 zumaManager_Position;
    private Vector3 path_Position;
    [HideInInspector] public int nowStage=1;
    private int lastStage = 1;

    // Start is called before the first frame update
    void Start()
    {
        boss_1_Position= boss_1.transform.position;
        zumaManager_Position = zumaManager.transform.position;
        path_Position = path.transform.position;
        boss_1.gameObject.SetActive(false);
        zumaManager.SetActive(false);
        path.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (lastStage != nowStage && nowStage == 2) 
        {
            ActiveBoss1();
            lastStage = nowStage;
        }
        if (lastStage != nowStage && nowStage == 3)
        {
            ActiveBoss2();
            lastStage = nowStage;
        }
    }
    private void ActiveBoss1()
    {
        boss_1.gameObject.SetActive(true);
    }


    private void ActiveBoss2()
    {
        //这里是一阶段死亡后生成二阶段的逻辑，想加动画的话在这里加
        OnEnterPhase2?.Invoke();
        path.SetActive(true);
        zumaManager.SetActive(true);
    }
    public void ResetBoss()
    {
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

