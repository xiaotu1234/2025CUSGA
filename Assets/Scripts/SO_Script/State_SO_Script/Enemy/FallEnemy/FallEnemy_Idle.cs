using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "FallEnemy_Idle", menuName = "ScriptableObject/Enemy/FallEnemy/FallEnemy_Idle")]
public class FallEnemy_Idle : FallEnemy
{
    private Animator animator;

    public float idleTime = 2f;
    private float idleTimer;

    public override void OnAwake()
    {
        base.OnAwake();
        animator = m_enemy.GetComponent<Animator>();

    }

    public override void OnEnter()
    {
        idleTimer = 0;
    }
    public override void OnUpdate()
    {
        idleTimer += Time.deltaTime;
        if(idleTimer > idleTime)
        {
            m_enemy.GetComponent<StateMachine>().TransitionState("FallEnemy_Seek");
        }
    }

    public override void OnExit()
    {
    }


}
