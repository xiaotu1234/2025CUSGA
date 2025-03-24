using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEnemyController : MonoBehaviour
{
    #region 冲刺敌人基本属性
    [SerializeField] private float m_maxHP;
    [SerializeField] private float damage;
    private float m_currentHP;
    
    #endregion

    public bool isAttacking = false;
    public event Action OnDashEnemyDead;
    private StateMachine m_fsm;
    private LayerMask m_layer;

    private void Awake()
    {
        m_currentHP = m_maxHP;
        m_fsm = GetComponent<StateMachine>();
        m_fsm.TransitionState("DashEnemy_Idle");
    }


    private void Update()
    {
        if (m_currentHP <= 0) 
        { 
            OnDashEnemyDead?.Invoke();
            return;
        }


    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        if (other.CompareTag("Player"))
            Debug.Log("Hurt Player");  // 调用玩家受伤的函数
    }


}
