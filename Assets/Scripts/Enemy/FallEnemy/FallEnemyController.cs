using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallEnemyController : EnemyController
{
    #region 冲刺敌人基本属性
    [Header("基本属性")]
    [SerializeField] private float damage;
    #endregion

    private StateMachine m_fsm;
    private void Awake()
    {
        m_currentHP = m_maxHP;
        m_fsm = GetComponent<StateMachine>();
    }

    void Start()
    {
        m_fsm.TransitionState("FallEnemy_Idle");
    }

    void Update()
    {
        
    }
    public float GetDamage()
    {
        return damage;
    }
    protected override void Die()
    {
        m_fsm.TransitionState("FallEnemy_Die");
        this.gameObject.tag = "SkillBall";
    }
}