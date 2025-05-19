using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallEnemyController : EnemyController
{
    #region �����˻�������
    [Header("��������")]
    [SerializeField] private float damage;
    #endregion

    
    

    protected override void Start()
    {
        base.Start();
        m_fsm.TransitionState("FallEnemy_Idle");
    }

    protected override void Update()
    {
        
    }
    public float GetDamage()
    {
        return damage;
    }
    protected override void Die()
    {
        base.Die();
        m_fsm.TransitionState("FallEnemy_Die");
        this.gameObject.tag = "SkillBall";
    }
}