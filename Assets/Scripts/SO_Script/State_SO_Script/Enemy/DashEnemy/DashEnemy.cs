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
        m_enemy = GameObject.Find("DashEnemy");
        m_player = GameObject.Find("Player");
        m_controller = m_enemy.GetComponent<DashEnemyController>();
        m_fsm = m_enemy.GetComponent<StateMachine>();
        if (m_controller == null ) 
            Debug.Log("Not Get ConTroller");
    }
}

