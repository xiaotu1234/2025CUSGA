using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss_1_Controller : MonoBehaviour
{
    public event Action<int> OnTentacleDie;
    public List<GameObject> enemyPrefs = new List<GameObject>();
    public List<GameObject> shootCube = new List<GameObject>();
    public List<Tentacle> tentacles = new List<Tentacle>();
    public BoxCollider produceEnemyArea;

    public StateMachine stateMachine;
    [HideInInspector]public bool isFirstStage = true;

    public int attackTentacleCount = 1;


    //策划加的动画
    public int tentaclesnum;
    public Animator chushoumain;
    public Animator chushouleft;
    public Animator chushouright;

    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        stateMachine.TransitionState("Boss_1_Produce");
        foreach (GameObject cube in shootCube)
        {
            cube.SetActive(false);
        }
    }

    private void OnEnable()
    {
        BossManager.Instance.OnBossDie += Die;
    }
    private void OnDisable()
    {
        BossManager.Instance.OnBossDie -= Die;
    }

    // Update is called once per frame
    void Update()
    {
        tentaclesnum = tentacles.Count;
        chushouleft.SetFloat("chushouNum", tentaclesnum);
        chushouright.SetFloat("chushouNum", tentaclesnum);
        if (tentacles.Count == 0)
        {
            Destroy(this.gameObject);
            BossManager.Instance.nowStage = 3;
        }
    }
    public void RemoveTentacle(Tentacle tentacle )
    {
        Debug.Log("RemoveTentacle");
        tentacles.Remove(tentacle);
        OnTentacleDie?.Invoke(1);
    }

    private void Die()
    {
        //这里启动死亡动画
    }


    public void CleanTentacle()
    {
        foreach (Tentacle tentacle in tentacles)
        {
            tentacle.Die();
        }
    }
    private void WaitForDestory()
    {
        //给动画事件调用
        Destroy(gameObject);
    }

}
