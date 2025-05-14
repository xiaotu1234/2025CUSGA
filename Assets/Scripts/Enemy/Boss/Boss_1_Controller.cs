using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss_1_Controller : MonoBehaviour
{
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
    public Animator chushou;


    void Start()
    {
        stateMachine.TransitionState("Boss_1_Produce");
        foreach (GameObject cube in shootCube)
        {
            cube.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        tentaclesnum = tentacles.Count;
        chushou.SetFloat("chushouNum", tentaclesnum);
        if (tentacles.Count == 0)
            Destroy(this.gameObject);
    }


}
