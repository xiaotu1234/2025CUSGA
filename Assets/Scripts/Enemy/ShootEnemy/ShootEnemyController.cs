using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEnemyController : EnemyController
{
    
    void Start()
    {
        base.Start();
        m_fsm.TransitionState("ShootEnemy_Idle");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected override void Die()
    {
        base.Die();
        m_fsm.TransitionState("ShootEnemy_Die");
        this.gameObject.tag = "SkillBall";
    }
}
