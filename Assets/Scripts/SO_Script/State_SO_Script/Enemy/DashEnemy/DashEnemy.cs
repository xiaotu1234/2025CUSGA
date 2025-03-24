using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DashEnemy : EnemyState
{ 
    protected GameObject m_player;
    protected DashEnemyController m_controller;
    protected StateMachine m_fsm;

    public override void OnAwake()
    {
        enemy = GameObject.Find("DashEnemy");
        m_player = GameObject.Find("Player");
        m_controller = enemy.GetComponent<DashEnemyController>();
        m_fsm = enemy.GetComponent<StateMachine>();
        if (m_controller == null ) 
            Debug.Log("Not Get ConTroller");
    }
}

