using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "FallEnemy_Idle", menuName = "ScriptableObject/Enemy/FallEnemy/FallEnemy_Idle")]
public class FallEnemy_Idle : FallEnemy
{
    private Animator animator;
    private GameObject scane;


    public override void OnAwake()
    {
        base.OnAwake();
        animator = enemy.GetComponent<Animator>();
        scane = enemy.transform.Find("Scane").gameObject;
    }

    public override void OnEnter()
    {
        throw new System.NotImplementedException();
    }
    public override void OnUpdate()
    {
        throw new System.NotImplementedException();
    }

    public override void OnExit()
    {
        throw new System.NotImplementedException();
    }


}
