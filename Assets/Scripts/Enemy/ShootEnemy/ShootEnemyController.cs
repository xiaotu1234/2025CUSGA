using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEnemyController : EnemyController
{

    protected override void Start()
    {
        base.Start();
        m_fsm.TransitionState("ShootEnemy_Idle");
    }


    protected override void Die()
    {
        base.Die();
        m_fsm.TransitionState("ShootEnemy_Die");
        this.gameObject.tag = "SkillBall";
    }
}
