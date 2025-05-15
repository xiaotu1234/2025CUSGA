using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DashEnemy : EnemyState
{ 
    protected PlayerController m_player;
    protected DashEnemyController m_controller;
    protected StateMachine m_fsm;
    

    public override void OnAwake()
    {
        m_player = PlayerManager.Instance.player;
        m_controller = m_enemy.GetComponent<DashEnemyController>();
        m_fsm = m_enemy.GetComponent<StateMachine>();
        m_animator = m_enemy.GetComponent<Animator>();
        if (m_controller == null ) 
            Debug.Log("Not Get ConTroller");
    }
}

